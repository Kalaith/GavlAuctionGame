using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class RegisterBidding : MonoBehaviour {

    House house;
    string house_uid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setHouse(House h, string uid) {
        // we will assume all auctions are ready to start now, later this should check if its started or not.
        house = h;
        house_uid = uid;
        registerAuctionBidding(uid);
    }

    public void loadAuction() {
        Debug.Log("Entering Auction: " + house_uid);

        LoadMenu menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
        menu.LoadGameAuction();
    }

    public void getBidderCount() {

    }

    public void registerAuctionBidding(string auction_uid) {
        Authentication auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));

        // Add an entry to the auction for a list of registered bidders
        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/" + auction_uid + "/bidders");

        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result[auth.getPlayerUID()] = auth.getPlayerName();

        dbref.UpdateChildrenAsync(result);

        dbref = FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID() + "/auctions");

        result = new Dictionary<string, System.Object>();
        result[auction_uid] = "auction_uid";

        dbref.UpdateChildrenAsync(result);
    }


}
