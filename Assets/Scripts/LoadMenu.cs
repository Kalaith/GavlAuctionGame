using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

    public GameObject loginPanel;
    public GameObject homePanel;
    public GameObject portfolioPanel;
    public GameObject auctionListPanel;
    public GameObject auctionPanel;
    public GameObject marketResearchPanel;
    public GameObject rankingPanel;
    public GameObject settingsPanel;

    public GameObject newsPanel;
    public GameObject menuPanel;

    // Use this for initialization
    void Start() {
        LoadGameLogin();
    }

    // Load Functions for each of the diffrent screens
    public void LoadGameLogin() {
        disableAllPanels();

        newsPanel.SetActive(false);
        menuPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void LoadGameHome() {
        disableAllPanels();

        homePanel.SetActive(true);
    }

    public void LoadGamePortfolio() {
        disableAllPanels();
        portfolioPanel.SetActive(true);
    }

    public void LoadGameAuctionList() {
        disableAllPanels();
        auctionListPanel.SetActive(true);
    }

    public void LoadGameAuction() {
        disableAllPanels();
        auctionPanel.SetActive(true);
    }

    public void LoadGameResearch() {
        disableAllPanels();
        marketResearchPanel.SetActive(true);
    }

    public void LoadGameRanking() {
        disableAllPanels();
        rankingPanel.SetActive(true);
    }

    public void LoadGameSettings() {
        disableAllPanels();
        settingsPanel.SetActive(true);
    }

    public void disableAllPanels() {
        loginPanel.SetActive(false);
        homePanel.SetActive(false);
        portfolioPanel.SetActive(false);
        auctionListPanel.SetActive(false);
        auctionPanel.SetActive(false);
        marketResearchPanel.SetActive(false);
        rankingPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // Lets enable this here, because only login disables these
        newsPanel.SetActive(true);
        menuPanel.SetActive(true);
    }
}
