using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Unity.Editor;

// Populate the database with test data
public class TestData : MonoBehaviour {

    DatabaseReference dbref;

    // Use this for initialization
    void Start () {
        dbref = FirebaseDatabase.DefaultInstance.RootReference;

        for (int i = 0; i < 10; i++) {
            //loadAuctionsIntoTest();
        }
        Debug.Log("Test Data Loaded.");
    }

    void loadAuctionsIntoTest() {
        string key = dbref.Child("auctions").Push().Key;
        AuctionListItem entry = new AuctionListItem(loadHousesIntoTest(), System.DateTime.Now);
        Dictionary<string, System.Object> auctionListItem = entry.ToDictionary();

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates["/auctions/" + key] = auctionListItem;

        dbref.UpdateChildrenAsync(childUpdates);
    }

    string loadHousesIntoTest() {
        string key = dbref.Child("houses").Push().Key;
        House entry = new House(key, 1, "Test Street", "0000");
        Dictionary<string, System.Object> auctionListItem = entry.ToDictionary();

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates["/houses/" + key] = auctionListItem;

        dbref.UpdateChildrenAsync(childUpdates);
        return key;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
