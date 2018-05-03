using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class LiveAuction : MonoBehaviour {

    public Text auction_log;
    public Text house_address;
    public Text winning_bidder;
    public Text current_price;

    public double price;
    private double bid_amount = 10000;
    Authentication auth;

    string current_auction_uid;

    Firebase.Database.DatabaseReference dbref;

    AuctionListItem item;

    // Use this for initialization
    void Start () {

        winning_bidder.text = "Opening Price";
        current_price.text = "0";

        // Object containing the authenticated user.
        auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));

        // Gets an auction, quick and dirty return the first.
        getPlayerAuction();


    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        } else {
            FirebaseDatabase.DefaultInstance.GetReference("auctions/" + current_auction_uid + "/")
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                        Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;

                        foreach (DataSnapshot child in snapshot.Children) {
                            Debug.Log(child.Key + "current_price");
                            if (child.Key.Equals("current_price")) {
                                price = System.Convert.ToDouble(child.Value);
                                current_price.text = "" + string.Format("{0:c}", price);
                                auction_log.text = auction_log.text + " New Bid, " + string.Format("{0:c}", bid_amount) + "\n";

                            }
                        }
                    }
                });
        }
        Debug.Log("HandleChildChanged" + args);
    }

    void getPlayerAuction() {
        FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID() + "/auctions").LimitToFirst(1)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions for player: "+ auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("Children: " + snapshot.ChildrenCount.ToString());
                    foreach (DataSnapshot child in snapshot.Children) {
                        current_auction_uid = child.Key;

                        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("auctions/" + current_auction_uid + "");
                        dbref.ChildChanged += HandleChildChanged;
                        Debug.Log(current_auction_uid);
                    }
                    getAuctionDetails();
                }
            });
    }

    void getAuctionDetails() {
        // Refrence to the database
        FirebaseDatabase.DefaultInstance.GetReference("auctions/" + current_auction_uid+"/")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot child in snapshot.Children) {
                        Debug.Log(child.Key + "current_price");
                        if(child.Key.Equals("current_price")) {
                            price = System.Convert.ToDouble(child.Value);
                            current_price.text = "$" + price.ToString();
                        }
                    }
                }
            });
        dbref.ChildChanged += HandleChildChanged;
        Debug.Log("Auction Ready");
    }

    // Update is called once per frame
    void Update () {
		
	}


    // Currently this should add the bid amount to the current price and update the database for it to be pushed out
    // Update the auction log with the bid information
    // add a log entry for this auction bid.
    // this is doing to many things atm, and should be reduced, writing to the onscreen log prob doesn't need to be done, an object should handle it from db.
    public void Bid() {

        Dictionary<string, System.Object> result;

        // Update the current price of the auction.
        result = new Dictionary<string, System.Object>();
        dbref = FirebaseDatabase.DefaultInstance.GetReference("auctions/" + current_auction_uid + "");
        result["current_price"] = price+ bid_amount;
        dbref.UpdateChildrenAsync(result);

        // Create a log of the bid and add to DB
        result["bidder_uid"] = auth.getPlayerUID();
        result["bid_amount"] = bid_amount;

        System.DateTime date = System.DateTime.Now;

        result["date"] = date.ToShortDateString();
        result["time"] = date.ToShortTimeString();

        // Get a current refrence to the db entry and update it
        dbref = FirebaseDatabase.DefaultInstance.GetReference("auction-log/" + current_auction_uid + "");
        // Create a new id for each log entry
        string key = dbref.Push().Key;
        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates[key] = result;

        dbref.UpdateChildrenAsync(childUpdates);

        Debug.Log("Bidding");
    }
}
