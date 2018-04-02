using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player script contains all the details for a single player, name, money on hand, housing portfolio
 */

public class Player {

    string displayName;
    string emailAddress;
    System.Uri photoUrl;
    string uid;

    public Player(string uid, string dn, string ea, System.Uri pu) {
        DisplayName = dn;
        EmailAddress = ea;
        PhotoUrl = pu;
    }

    public string DisplayName {
        get {
            return displayName;
        }

        set {
            displayName = value;
        }
    }

    public string EmailAddress {
        get {
            return emailAddress;
        }

        set {
            emailAddress = value;
        }
    }

    public System.Uri PhotoUrl {
        get {
            return photoUrl;
        }

        set {
            photoUrl = value;
        }
    }

    public string Uid {
        get {
            return uid;
        }

        set {
            uid = value;
        }
    }
}
