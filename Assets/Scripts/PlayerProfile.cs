using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Database;

public class PlayerProfile : MonoBehaviour {

    public Text playerName;
    public Text PortfolioValue;
    public Text walletValue;
    public Text housesOwned;
    Authentication auth;

    // Use this for initialization
    void Start () {

        // Object containing the authenticated user.
        auth = (Authentication)GameObject.Find("Authentication").GetComponent(typeof(Authentication));

        playerName.text = auth.p.DisplayName;
        PortfolioValue.text = auth.p.PortfolioValue.ToString();
        walletValue.text = auth.p.CashOnHand.ToString();
        housesOwned.text = auth.p.HousesOwned.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
