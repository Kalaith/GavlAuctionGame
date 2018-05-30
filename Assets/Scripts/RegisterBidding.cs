using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
public class RegisterBidding : MonoBehaviour {

    House house;
    string auction_uid;
    int bidders;
    bool registered;
    Authentication auth;

    public GameObject address;
    public GameObject suburb;

    public GameObject starting_price;
    public GameObject current_price;

    public GameObject register;

    public GameObject registered_bidders;
    Text bidderTxt;

	// Use this for initialization
	void Start () {
        bidders = 0;
        registered = false;

        auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));
        bidderTxt = registered_bidders.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        if (bidders == 1) {
            bidderTxt.text = bidders + " live participant";
        } else {
            bidderTxt.text = bidders + " live participants";
        }
	}

    public void setHouse(House h, string uid) {
        // we will assume all auctions are ready to start now, later this should check if its started or not.
        house = h;
        auction_uid = uid;

        Debug.Log("House UID: "+uid);

        registerAuctionBidding(uid);

        findBidderCount(uid);

        getHouseAuctionInformation();

    }

    public void loadAuction() {
        Debug.Log("Entering Auction: " + auction_uid);

        LoadMenu menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
        menu.LoadGameAuction();
    }

    public void findBidderCount(string uid) {
        Debug.Log("Record number of bidders");
        Firebase.Database.DatabaseReference houses = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/"+ uid+"/bidders");

        houses.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("Unable to retreive auctions");
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Finding a list of bidders.");
                bidders = 0;
                foreach (DataSnapshot child in snapshot.Children) {
                    Debug.Log("Bidder: "+child.Key);
                    bidders++;
                    // if the player is in this list they are already registered
                    if(child.Key.Equals(auth.getPlayerUID())) {
                        registered = true;
                        Debug.Log("Player is registered");
                    }
                    /*
                    IDictionary house = (IDictionary)child.Value;
                    owner = (string)house["bidders"];
                    string address = (string)house["address"];
                    string suburb = (string)house["suburb"];

                    // we should return here but this is okay
                    if (owner == null && house_uid.Equals("")) {
                        house_uid = child.Key.ToString();

                    }*/

                }
            }
        });
    }

    public void registerAuctionBidding(string auction_uid) {
        auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));
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

    void getHouseAuctionInformation() {

        Firebase.Database.DatabaseReference house_auction = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/"+ auction_uid);

        house_auction.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("Unable to retreive auction information");
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log("getHouseAuctionInformation");
                string add = "";
                string sub = "";
                string c_price = "";
                string s_price = "";

                foreach (DataSnapshot child in snapshot.Children) {
                    Debug.Log("Auction Info Found");
                    if (child.Key.Equals("address")) {
                        add = child.Value.ToString();
                    }
                    if (child.Key.Equals("suburb")) {
                        sub = child.Value.ToString();
                    }
                    if (child.Key.Equals("current_price")) {
                        c_price = child.Value.ToString();
                    }
                    if (child.Key.Equals("starting_price")) {
                        s_price = child.Value.ToString();
                    }
                }
                Debug.Log("getHouseAuctionInformation2");
                updateHouseInfo(add, sub, c_price, s_price);

            }
        });

    }

    public void updateHouseInfo(string a, string s, string cp, string sp) {
        Debug.Log("Update screen");
        Text addressTxt = address.gameObject.GetComponent<Text>();
        addressTxt.text = a;
        Text suburbTxt = suburb.GetComponent<Text>();
        suburbTxt.text = s;
        Text current_priceTxt = current_price.gameObject.GetComponent<Text>();
        current_priceTxt.text = string.Format("{0:c}", cp);
        Text starting_priceTxt = starting_price.GetComponent<Text>();
        starting_priceTxt.text = string.Format("{0:c}", sp);
    }
}
