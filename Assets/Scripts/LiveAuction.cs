using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class LiveAuction : MonoBehaviour {

    public Text auction_text;
    public Text house_address;
    public Text house_suburb;

    public Text winning_bidder;
    public Text current_price;
    public Text player_price;
    public Text time_remaining;

    public double price;
    private double bid_amount = 10000;
    private double current_player_bid = 0;

    Authentication auth;

    string current_auction_uid;

    // this is set to the start time + 10mins
    System.DateTime auction_date;
    // This is set to the end of the auction time.
    System.DateTime auction_close_date;


    
    Firebase.Database.DatabaseReference auction;
    Firebase.Database.DatabaseReference auction_log;

    // How many bids
    int bids;
    // How many bids past the 10min mark
    int overtime_bids;

    bool closed;
    string house_uid;
    float timeLeft;
    float minutes;
    float seconds;


    int bidders;

    System.TimeSpan remaining;

    public GameObject participants;
    Text particpantTxt;

    public AudioClip welcome;
    public AudioClip closed_win;
    public AudioClip closed_loss;


    AudioSource audioSource;

    // Use this for initialization
    void Start () {

        bids = 0;
        bidders = 0;
        // Object containing the authenticated user.
        auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));

        particpantTxt = participants.GetComponent<Text>();

        // Gets an auction, quick and dirty return the first.
        getPlayerAuction();
        timeLeft = 600;


        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(welcome, 1F);


    }

    // Update is called once per frame
    void Update() {

        remaining = auction_close_date-System.DateTime.Now;
        // stop the timer counting down at end of the auction.
        if (remaining.TotalSeconds <= 0) {
            time_remaining.text = "00:00 remaining";
        } else {
            time_remaining.text = remaining.Minutes + ":" + remaining.Seconds + " remaining";
        }

        
        if (bidders == 1) {
            particpantTxt.text = bidders + " participant";
        } else {
            particpantTxt.text = bidders + " participants";
        }
    }

    /// <summary>
    /// Since this is currently client driven we will handle closing the auction from here, for a production system this would need to be moved to avoid cheating.
    /// whats happens when there is more then one bidder...
    /// </summary>
    void closeAuction() {
        Dictionary<string, System.Object> result;

        // Add the auction to the list of closed auctions
        result = new Dictionary<string, System.Object>();
        auction = FirebaseDatabase.DefaultInstance.GetReference("closed-auctions/" + current_auction_uid + "");
        result["closed"] = true;
        result["house_uid"] = house_uid;
        result["last_bidder"] = auth.getPlayerUID();
        result["price"] = price;
        result["date"] = auction_close_date.ToShortDateString();
        result["time"] = auction_close_date.ToShortTimeString();

        Debug.Log("Updating closed auction");
        auction.UpdateChildrenAsync(result);

        // Remove the auction from the live auctions.
        auction = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/");
        Debug.Log("removing auction: "+ current_auction_uid);
        auction.Child(current_auction_uid).RemoveValueAsync();

        // Remove the auction from the player.
        Firebase.Database.DatabaseReference player = FirebaseDatabase.DefaultInstance.GetReference("player/"+ auth.getPlayerUID()+"/auctions");
        Debug.Log("removing auction: " + current_auction_uid + " from player " + auth.getPlayerUID());
        player.Child(current_auction_uid).RemoveValueAsync();
    }

    /// <summary>
    /// Check to see if this auction has gone into overtime.
    /// </summary>
    bool isAuctionOver() {
        remaining = auction_close_date - System.DateTime.Now;

        if (remaining.TotalSeconds <= 0) {
            Debug.Log("Time to close the auction"+ remaining.TotalSeconds);
            return true;
        } else {
            Debug.Log("not yet time to close auction" + remaining.TotalSeconds);
        }

        // We have not gone overtime yet
        return false;
        
    }

    // if the auction is closed, then the winner is the one with the last bid.
    void isWinner(string winner_uid) {
        Debug.Log("Did someone win");
        // only delclare them the winner if the auction is over and their ID matches the last bidder id.
        if (isAuctionOver()) {
            Debug.Log("Did the player win");
            if (winner_uid == auth.getPlayerUID()) {
                Debug.Log("we won");
                audioSource.PlayOneShot(closed_win, 1F);

                Firebase.Database.DatabaseReference player_ref = FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID());

                // Update the players current cash available if they won the auction
                Dictionary<string, System.Object> update_player = new Dictionary<string, System.Object>();

                // Add the price we paid for the house, dont need the rest of the details.
                auth.p.CashOnHand -= price;
                update_player["cash_on_hand"] = auth.p.CashOnHand;

                player_ref.UpdateChildrenAsync(update_player);

                // Assign the property to the player
                Firebase.Database.DatabaseReference player_property = FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID() + "/property");

                // Update the current price of the auction.
                Dictionary<string, System.Object> purchase_house = new Dictionary<string, System.Object>();

                // Add the price we paid for the house, dont need the rest of the details.
                purchase_house["purchase_price"] = price;
 
                Dictionary<string, System.Object> house_update = new Dictionary<string, System.Object>();
                house_update[house_uid] = purchase_house;

                player_ref.UpdateChildrenAsync(house_update);

                // Update the house to have a owner.
                Firebase.Database.DatabaseReference house_ref = FirebaseDatabase.DefaultInstance.GetReference("houses/" + house_uid);

                // Update the current price of the auction.
                Dictionary<string, System.Object> house = new Dictionary<string, System.Object>();

                // Add the price we paid for the house, dont need the rest of the details.
                house["owner"] = auth.getPlayerUID();

                //Dictionary<string, System.Object> house_update = new Dictionary<string, System.Object>();
                //house_update[house_uid] = purcahse_house;

                house_ref.UpdateChildrenAsync(house);

                // After we have updated the auction for the winner, close the auction.
                closeAuction();
                // TODO Work out how much money the player has, deduct the house price - the loan amount the equity on the house they already own
            } else {
                Debug.Log("auction over player didn't win");
                audioSource.PlayOneShot(closed_loss, 1F);

            }
        }
    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        } else {
            auction.GetValueAsync().ContinueWith(task => {

                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot child in snapshot.Children) {
                        Debug.Log(child.Key + "current_price");
                        if (child.Key.Equals("current_price")) {
                            //price = System.Convert.ToDouble(child.Value);
                            //current_price.text = "" + string.Format("{0:c}", price);
                        }
                    if (child.Key.Equals("closed")) {
                        closed = System.Convert.ToBoolean(child.Value);
                        //current_price.text = "" + string.Format("{0:c}", price);
                    }
                    if(child.Key.Equals("last_bidder")) {
                        isWinner(child.Value.ToString());
                    }

                }
                }

            });
        }
        Debug.Log("HandleChildChanged");
    }

    public void findBidderCount() {
        Firebase.Database.DatabaseReference current_bidders = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/" + current_auction_uid + "/bidders");

        current_bidders.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("Unable to retreive auctions");
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Finding a count of bidders.");
                bidders = (int)snapshot.ChildrenCount;
            }
        });
    }


    // When a new bid comes in we should update the expected finish time, the current price and how many bids over time.
    void HandleNewBid(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        } else {
            auction_log.GetValueAsync().ContinueWith(task => {

                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    price += bid_amount;
                    current_price.text = "" + string.Format("{0:c}", price);

                    auction_text.text += "<color=#007e83>New Bid:</color> " + string.Format("{0:c}", bid_amount)+ "\r\n";
                    bids++;

                    remaining = auction_close_date - System.DateTime.Now;

                    // Only add time if in the last 30seconds - overtime bids but greater then 0 seconds remaining
                    if (remaining.TotalSeconds > 0 && remaining.TotalSeconds < (30 - overtime_bids)) {
                        // Will the date of bid + 30 seconds put it past the current auction time, if so add 30 seconds - the number of bids over time, we wont reach here if we are already over time, if we are overtime such as negative seconds it should get caught.
                        auction_close_date = auction_close_date.AddSeconds(30 - overtime_bids);
                        overtime_bids++;
                    }
                    
                }

                });
        }
        Debug.Log("HandleNewBid" + args);
    }

    void getPlayerAuction() {
        FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID() + "/auctions").LimitToFirst(1)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions for player: "+ auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
 
                    foreach (DataSnapshot child in snapshot.Children) {
                        current_auction_uid = child.Key;

                        FirebaseDatabase.DefaultInstance.GetReference("live-auctions/" + current_auction_uid + "").ChildChanged += HandleChildChanged;
                    }
                    getAuctionDetails();
                }
            });
    }

    void getAuctionDetails() {
        Debug.Log("Auction Details" + current_auction_uid);
        // Refrence to the database
        auction = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/" + current_auction_uid + "");
        auction.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                System.DateTime auction_time = System.DateTime.Now;

                if (snapshot.ChildrenCount == 0) {
                    Debug.Log("Not registered to bid in any upcoming auctions -"+ current_auction_uid + "-"+ snapshot.ChildrenCount);
                    LoadMenu menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
                    menu.LoadGameHome();
                } else {
                    Debug.Log("Auction Details");
                    foreach (DataSnapshot child in snapshot.Children) {
                        if (child.Key.Equals("current_price")) {
                            price = System.Convert.ToDouble(child.Value);
                            current_price.text = string.Format("{0:c}", price);
                        }
                        if (child.Key.Equals("date")) {
                            auction_date = System.Convert.ToDateTime(child.Value);
                            current_price.text = string.Format("{0:c}", price);
                        }
                        if (child.Key.Equals("time")) {
                            auction_time = System.Convert.ToDateTime(child.Value);

                            //auction_time = auction_time.AddMinutes(10);
                            current_price.text = string.Format("{0:c}", price);
                        }
                        if (child.Key.Equals("house_uid")) {
                            house_uid = child.Value.ToString();
                        }
                        if (child.Key.Equals("last_bidder")) {
                            isWinner(child.Value.ToString());
                        }

                        if (child.Key.Equals("address")) {
                            house_address.text = child.Value.ToString();
                        }
                        if (child.Key.Equals("suburb")) {
                            house_suburb.text = child.Value.ToString();
                        }
                    }

                    auction_date = auction_date.Add(auction_time.TimeOfDay);
                    auction_close_date = auction_date.AddMinutes(10);

                    findBidderCount();

                    Debug.Log("Auction Close Time: "+auction_close_date);
                }
            }
            });
        auction.ChildChanged += HandleChildChanged;

        // Refrence to the database
        auction_log = FirebaseDatabase.DefaultInstance.GetReference("auction-log/" + current_auction_uid + "/");
        auction_log.ChildAdded += HandleNewBid;
        // We should also read in all the entries so they can be posted to the log and update the price to match
        Debug.Log("Auction Ready");


    }


    // Currently this should add the bid amount to the current price and update the database for it to be pushed out
    // Update the auction log with the bid information
    // add a log entry for this auction bid.
    // this is doing to many things atm, and should be reduced, writing to the onscreen log prob doesn't need to be done, an object should handle it from db.
    public void Bid() {

        // Check to see if we can bid and if hasn't gone past the auction time.
        System.DateTime date = System.DateTime.Now;
        Debug.Log(date);
        Debug.Log(auction_close_date);
        
        // is the auction currently open and does the player have enough cash on hand to pay for it.
        if (date < auction_close_date) {
            // currently this would not stop them from duel bidding on auctions, so the game shouldn't allow it.
            if (auth.p.CashOnHand >= (price + bid_amount)) {
                // update how much the player has bid.
                current_player_bid += bid_amount;

                player_price.text = (string.Format("{0:c}", (price + bid_amount)));

                Dictionary<string, System.Object> auction_result = new Dictionary<string, System.Object>();

                // Add who was the last bidder and the current price its at
                auction_result["last_bidder"] = auth.getPlayerUID();
                //auction_result["current_price"] = price+bid_amount;

                auction.UpdateChildrenAsync(auction_result);

                Dictionary<string, System.Object> log_result;

                // Update the current price of the auction.
                log_result = new Dictionary<string, System.Object>();
                //result["current_price"] = price + bid_amount;
                //auction.UpdateChildrenAsync(result);

                // Create a log of the bid and add to DB
                log_result["bidder_uid"] = auth.getPlayerUID();
                //log_result["bid_amount"] = bid_amount;

                log_result["date"] = date.ToShortDateString();
                log_result["time"] = date.ToShortTimeString();

                // Create a new id for each log entry
                string key = auction_log.Push().Key;
                Dictionary<string, System.Object> log_update = new Dictionary<string, System.Object>();
                log_update[key] = log_result;

                auction_log.UpdateChildrenAsync(log_update);


                Debug.Log("Bidding");
            } else {
                Debug.Log("Not enough money to keep bidding.");
                Debug.Log("Player Cash: "+auth.p.CashOnHand);
            }
        } else {
            Debug.Log("Unable to bid, auction over.");

            isWinner(auth.getPlayerUID());

        }
    }
}
