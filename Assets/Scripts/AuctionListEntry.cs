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

    Firebase.Database.DatabaseReference dbref;

    public void SetText(string uid, string auid) {
        entry.text = uid;
        auction_uid = auid;
        Debug.Log("A UID: " + auction_uid);
    }

    /// <summary>
    /// Pass the onclick back to the AuctionList to handle
    /// </summary>
    public void OnClick() {
        Authentication auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));

        // Add an entry to the auction for a list of registered bidders
        dbref = FirebaseDatabase.DefaultInstance.GetReference("live-auctions/"+ auction_uid+"/bidders");

        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result[auth.getPlayerUID()] = auth.getPlayerName();

        dbref.UpdateChildrenAsync(result);

        dbref = FirebaseDatabase.DefaultInstance.GetReference("player/" + auth.getPlayerUID() + "/auctions");

        result = new Dictionary<string, System.Object>();
        result[auction_uid] = entry.text;

        dbref.UpdateChildrenAsync(result);
    }


}
