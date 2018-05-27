using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class AuctionList : MonoBehaviour {


    Dictionary<string, AuctionListItem> auction_list = new Dictionary<string, AuctionListItem>();

    public GameObject Auction1;
    public GameObject Auction2;
    public GameObject Auction3;
    public GameObject Auction4;
    public GameObject Auction5;
    public GameObject Auction6;

    int auction_count = 0;

    // Use this for initialization
    void Start () {
        // do a check and create a auction if none exist
        findNewAuction();

        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("live-auctions");

        dbref.ChildAdded += HandleChildAdded;
        dbref.ChildChanged += HandleChildChanged;
        dbref.ChildRemoved += HandleChildRemoved;
        dbref.ChildMoved += HandleChildMoved;

        GameObject auth = GameObject.Find("Authentication");

        Authentication other = (Authentication)auth.GetComponent(typeof(Authentication));

        Debug.Log(other.p.Uid);

        listActions();
    }

    // Function to find a house for sale and create a new auction that starts in 1 min, if no auctions exist
    void findNewAuction() {
        FirebaseDatabase.DefaultInstance
            .GetReference("live-auctions").OrderByChild("date")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to retreive auctions");
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // If we dont have any auctions, create 1
                    if (snapshot.ChildrenCount == 0) {
                        // Find a house for sale
                        Debug.Log("Need to find and create an auction");
                        getHouseForSale();
                    }
                }
            });
    }

    void createAuction(string house_uid) {
        Debug.Log("Creating a new auction "+house_uid);
        Firebase.Database.DatabaseReference auctions = FirebaseDatabase.DefaultInstance.GetReference("live-auctions");
        string key = auctions.Push().Key;
        AuctionListItem entry = new AuctionListItem(house_uid, System.DateTime.Now, 0, 100000);
        Dictionary<string, System.Object> auctionListItem = entry.ToDictionary();

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates[key] = auctionListItem;

        auctions.UpdateChildrenAsync(childUpdates);
    }

    // Assumes we will always find a house to sell.
    void getHouseForSale() {

        Debug.Log("Finding house without a owner");
       Firebase.Database.DatabaseReference houses = FirebaseDatabase.DefaultInstance.GetReference("houses/");
        string house_uid = "";
        string owner= "";
        houses.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("Unable to retreive auctions");
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Finding a house.");
                foreach (DataSnapshot child in snapshot.Children) {
                    
                    IDictionary house = (IDictionary)child.Value;
                    owner = (string)house["owner"];

                    // we should return here but this is okay
                    if (owner == null && house_uid.Equals("")) {
                        house_uid = child.Key.ToString();
                        Debug.Log("Unowned house found: " + house_uid);
                        createAuction(house_uid);
                    }

                }
            }
        });

    }

    // This list will show all the current open auctions, what time they are on, maybe a price range,
    // a user should be able to click on the auction and see more information, they should also be able to register to bid
    // If an auction is on when they visit the screen they should be taken to the auction in process.
    // Need to be able to get the list, have it refresh, register for bid and view 
    void listActions() {
        /*FirebaseDatabase.DefaultInstance
            .GetReference("live-auctions").OrderByChild("date")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to retreive auctions");
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.ChildrenCount == 0) {
                        Debug.Log("No auction listings available");                      
                    }

                    foreach (DataSnapshot child in snapshot.Children) {
                        // Create a event from the data retrieved
                        createAuctionListItem(child);
                    }

                }
        });*/
    }

    public void viewHouseAuction(string uid) {
        Debug.Log("House clicked: "+uid);
        LoadMenu menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
        menu.LoadAuctionHouse();
        RegisterBidding bid = (RegisterBidding)GameObject.Find("HouseAuctionScreen").GetComponent(typeof(RegisterBidding));
        bid.setHouse(uid);
    }

    // populates the game object auction list item with the auction details
    void setAuctionListItem(GameObject auction, string house_uid, string add, string sub, string pri, string da) {
        Text address = auction.transform.Find("Address").gameObject.GetComponent<Text>();
        address.text = add;
        Text suburb = auction.transform.Find("Suburb").gameObject.GetComponent<Text>();
        suburb.text = sub;
        Text price = auction.transform.Find("HousePrice").gameObject.GetComponent<Text>();
        price.text = pri;
        Text auctionDate = auction.transform.Find("AuctionDate").gameObject.GetComponent<Text>();
        auctionDate.text = da;
        // Add an event listener which when clicked calls to view the house.
        auction.SetActive(true);
        Debug.Log("House: " + house_uid);
        auction.GetComponent<Button>().onClick.AddListener(() => viewHouseAuction(house_uid));
        Debug.Log("House2: " + house_uid);
    }

    // Gets a new entry into the auction list screen
    void createAuctionListItem(DataSnapshot child) {
        // Create a event from the data retrieved
        Debug.Log("Creating new child listing: " + child.Key);
        IDictionary dictUser = (IDictionary)child.Value;
        AuctionListItem item = new AuctionListItem(dictUser);
        auction_list[child.Key] = item;

        auction_count++;
        if (auction_count == 1) {
            setAuctionListItem(Auction1, child.Key, "123 Test Street", "Suburb", "$100,000", "27/5/2018 11:00PM");
            Auction1.SetActive(true);
        }
        if (auction_count == 2) {
            Auction2.SetActive(true);
        }
        if (auction_count == 3) {
            Auction3.SetActive(true);
        }
        if (auction_count == 4) {
            Auction4.SetActive(true);
        }
        if (auction_count == 5) {
            Auction5.SetActive(true);
        }
        if (auction_count == 6) {
            Auction6.SetActive(true);
        }
        // Create a game object, currently just a button, should be something fancier later.
        //GameObject entry = Instantiate(eventTemplate) as GameObject;
        //entry.SetActive(true);

        //entry.GetComponent<AuctionListEntry>().SetText(item.ToString(), child.Key);
        //entry.transform.SetParent(eventTemplate.transform.parent, false);


    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildChanged" + args);
    }

    // This is called when a child is added, its also called when first loaded for exisiting entries.
    void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        createAuctionListItem(args.Snapshot);
    }

    void HandleChildRemoved(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildRemoved" + args);
        // TODO need to be able to remove the game object for this auction.
    }

    void HandleChildMoved(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildMoved" + args);
    }

    // Update is called once per frame
    void Update () {
		
	}

}
