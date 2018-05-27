using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

/*
 * Player script contains all the details for a single player, name, money on hand, housing portfolio
 */

public class Player {

    string displayName;
    string emailAddress;
    System.Uri photoUrl;
    string uid;
    double cashOnHand;
    double portfolioValue;
    int housesOwned;

    public Player(string uid, string dn, string ea, System.Uri pu, double coh, double pv, int ho) {
        this.uid = uid;
        DisplayName = dn;
        EmailAddress = ea;
        PhotoUrl = pu;
        cashOnHand = coh;
        portfolioValue = pv;
        housesOwned = ho;
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

    public double CashOnHand {
        get {
            return cashOnHand;
        }

        set {
            cashOnHand = value;
        }
    }

    public double PortfolioValue {
        get {
            return portfolioValue;
        }

        set {
            portfolioValue = value;
        }
    }

    public int HousesOwned {
        get {
            return housesOwned;
        }

        set {
            housesOwned = value;
        }
    }


    // Gets the players cash on hand and uses it to update the play field, can be called to update
    public void updatePlayerHousesOwned() {

        FirebaseDatabase.DefaultInstance.GetReference("player/" + uid + "/houses")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions for player: " + uid);
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot child in snapshot.Children) {
                        Debug.Log("House Key" + child.Key);
                        Debug.Log("House Value" + child.Key);
                        housesOwned = (int)child.Value;
                    }
                }
            });
    }

    public void updatePlayerPortfolioValue() {

        FirebaseDatabase.DefaultInstance.GetReference("player/" + uid + "/houses")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Unable to get auctions for player: " + uid);
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot child in snapshot.Children) {
                        Debug.Log("House Key" + child.Key);
                        Debug.Log("House Value" + child.Key);
                        housesOwned = (int)child.Value;
                    }
                }
            });
    }

}
