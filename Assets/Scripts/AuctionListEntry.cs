using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;

public class AuctionListEntry : MonoBehaviour {

    [SerializeField]
    private Text entry;
    private string auction_uid;

    public void SetText(string uid, string auid) {
        entry.text = uid;
        auction_uid = auid;
        Debug.Log("A UID: " + auction_uid);
    }

    /// <summary>
    /// Pass the onclick back to the AuctionList to handle
    /// </summary>
    public void OnClick() {
        Debug.Log("I have been clicked " + auction_uid);
        GameObject auth = GameObject.Find("Authentication");

        Authentication other = (Authentication)auth.GetComponent(typeof(Authentication));

        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("auction-bidders");

        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["auction_uid"] = auction_uid;
        result["player_uid"] = other.getPlayerUID();

        string key = dbref.Child("auction-bidders").Push().Key;
        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates[key] = result;

        Debug.Log("Key: " + key);
        Debug.Log("A UID: " + auction_uid);
        Debug.Log("P UID: " + other.getPlayerUID());

        dbref.UpdateChildrenAsync(childUpdates);
    }


}
