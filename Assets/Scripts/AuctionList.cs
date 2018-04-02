using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class AuctionList : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Firebase.Database.DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference("auctions");

        dbref.ChildAdded += HandleChildAdded;
        dbref.ChildChanged += HandleChildChanged;
        dbref.ChildRemoved += HandleChildRemoved;
        dbref.ChildMoved += HandleChildMoved;
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
