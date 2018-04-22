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

    Authentication auth;

    string current_auction;

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
                            current_auction = child.Key;
                        }
                        getAuctionDetails();
                    }
                });
    }

    void getAuctionDetails() {
        // Refrence to the database
        dbref.GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions: " + auth.getPlayerUID());
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("Children: " + snapshot.ChildrenCount.ToString());
                    foreach (DataSnapshot child in snapshot.Children) {
                        IDictionary dictUser = (IDictionary)child.Value;
                        item = new AuctionListItem(dictUser);
                        Debug.Log("Price: " + item.current_price);
                    }
                }
            });
        dbref.ChildChanged += HandleChildChanged;
        Debug.Log("Auction Ready");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Bid() {

        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        price += 10000;
        current_price.text = "$"+price.ToString();
        auction_log.text = auction_log.text + auth.getPlayerName()+" New Bid, $10, 000\n";

        result["current_price"] = price;

        dbref.UpdateChildrenAsync(result);

        Debug.Log("Bidding");
    }
}
