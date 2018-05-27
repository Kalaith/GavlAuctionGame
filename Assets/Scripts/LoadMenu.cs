using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

    public GameObject loginPanel;
    public GameObject homePanel;
    public GameObject profileScreen;
    public GameObject portfolioScreen;
    public GameObject auctionListScreen;
    public GameObject auctionPanel;
    public GameObject UIPanel;
    public GameObject splashScreen;
    public GameObject HouseDetailsScreen;
    public GameObject auctionHouseScreen;

    // Use this for initialization
    void Start() {
        Debug.Log("Starting the game");

        StartCoroutine(LoadGameLogin());
    }

    // Load Functions for each of the diffrent screens
    public IEnumerator LoadGameLogin() {
        disableAllPanels();

        splashScreen.SetActive(true);

        yield return new WaitForSeconds(3);
        splashScreen.SetActive(false);
        loginPanel.SetActive(true);

    }

    public void LoadAuctionHouse() {
        disableAllPanels();

        UIPanel.SetActive(true);
        auctionHouseScreen.SetActive(true);
    }

    public void LoadGameHome() {
        disableAllPanels();

        UIPanel.SetActive(true);
        homePanel.SetActive(true);
    }

    public void LoadProfileScreen() {
        disableAllPanels();

        UIPanel.SetActive(true);
        profileScreen.SetActive(true);
    }

    public void LoadGamePortfolio() {
        disableAllPanels();

        UIPanel.SetActive(true);
        portfolioScreen.SetActive(true);
    }

    public void LoadGameAuctionList() {
        disableAllPanels();

        UIPanel.SetActive(true);
        auctionListScreen.SetActive(true);
    }

    public void LoadGameAuction() {
        disableAllPanels();

        UIPanel.SetActive(true);
        auctionPanel.SetActive(true);
    }



    public void disableAllPanels() {
        splashScreen.SetActive(false);
        UIPanel.SetActive(false);
        loginPanel.SetActive(false);
        homePanel.SetActive(false);
        profileScreen.SetActive(false);
        portfolioScreen.SetActive(false);
        auctionListScreen.SetActive(false);
        auctionPanel.SetActive(false);
        auctionHouseScreen.SetActive(false);
    }
}
