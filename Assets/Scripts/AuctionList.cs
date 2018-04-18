using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class AuctionList : MonoBehaviour {


    Dictionary<string, AuctionListItem> auction_list = new Dictionary<string, AuctionListItem>();

    [SerializeField]
    private GameObject eventTemplate;

    // Use this for initialization
    void Start () {
        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("auctions");

        dbref.ChildAdded += HandleChildAdded;
        dbref.ChildChanged += HandleChildChanged;
        dbref.ChildRemoved += HandleChildRemoved;
        dbref.ChildMoved += HandleChildMoved;

        GameObject auth = GameObject.Find("Authentication");

        Authentication other = (Authentication)auth.GetComponent(typeof(Authentication));

        Debug.Log(other.p.Uid);

        listActions();
    }

    // This list will show all the current open auctions, what time they are on, maybe a price range,
    // a user should be able to click on the auction and see more information, they should also be able to register to bid
    // If an auction is on when they visit the screen they should be taken to the auction in process.
    // Need to be able to get the list, have it refresh, register for bid and view 
    void listActions() {
        FirebaseDatabase.DefaultInstance
            .GetReference("auctions").OrderByChild("date")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to retreive auctions");
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("Children: " + snapshot.ChildrenCount.ToString());
                    foreach (DataSnapshot child in snapshot.Children) {
                        // Create a event from the data retrieved
                        IDictionary dictUser = (IDictionary)child.Value;
                        AuctionListItem item = new AuctionListItem(dictUser);
                        auction_list[child.Key] = item;

                        // Create a game object, currently just a button, should be something fancier later.
                        GameObject entry = Instantiate(eventTemplate) as GameObject;
                        entry.SetActive(true);
                        
                        entry.GetComponent<AuctionListEntry>().SetText(item.ToString(), child.Key);
                        entry.transform.SetParent(eventTemplate.transform.parent, false);

                    }
                }
        });
    }

    public void onBtnClicked(string auid) {
        
        /**/

    }

    void HandleChildChanged(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildChanged" + args);
    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildAdded" + args);
    }

    void HandleChildRemoved(object sender, ChildChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log("HandleChildRemoved" + args);
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
