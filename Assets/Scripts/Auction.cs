using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
using LocalNotification = UnityEngine.iOS.LocalNotification;
//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;

public class Auction : MonoBehaviour {
	public GameObject LogText;
	public GameObject CurrentPrice;
	public int bidnumber = 100000;
	public int bidincrease;
	//bidnumber is the real bid number fetched from Firebase server.
	public Button BidBtn;
	public int userbalance = 500000;
	public int housevalue;
	public int othersbid = 100000;
	public int othersbidincrease;
	// othersbid is randomized variable to decide others follow your bid or hold.
	public bool auctionopen = false;
	public bool auctionwait = false;
	public bool auctionend = false;
	public float auctiontime;

	public float luxuryrate = 1;
	public float randomluxuryrate;
	public double agentfees = 10000;
	public int reserveprice;
	public int anverageprice = 50000;
	public int housesupply = 10;
	public string randomnewstext;

	void start (){

	}

	void Update(){
		if (Time.time > 2 && auctionwait == false) {
			auctionopen = true;
		}

		//check pre-auction wait is over or not
		if (auctionwait == true && (Time.time - auctiontime) > 10) {
			auctionend = true;
		}
		//check auction is over or not
		if (NotificationServices.localNotificationCount > 0) {
			Debug.Log(NotificationServices.localNotifications[0].alertBody);
			NotificationServices.ClearLocalNotifications();
		}
		//push iOS notifications
		if ((Time.time / 60) % 5 == 0){
			var notif = new LocalNotification();
			notif.fireDate = System.DateTime.Now.AddSeconds(0);
			var rad = (int) Random.Range (0, 9);
			switch (rad) 
			{
			case 0:
				randomnewstext = "NewsTexts0";
				break;
			case 1:
				randomnewstext = "NewsTexts1";
				break;
			case 2:
				randomnewstext = "NewsTexts2";
				break;
			case 3:
				randomnewstext = "NewsTexts3";
				break;
			case 4:
				randomnewstext = "NewsTexts4";
				break;
			case 5:
				randomnewstext = "NewsTexts5";
				break;
			case 6:
				randomnewstext = "NewsTexts6";
				break;
			case 7:
				randomnewstext = "NewsTexts7";
				break;
			case 8:
				randomnewstext = "NewsTexts8";
				break;
			case 9:
				randomnewstext = "NewsTexts9";
				break;
			}
			notif.alertBody = randomnewstext;
			NotificationServices.ScheduleLocalNotification(notif);
			// random news event iOS notification to be delivered for every 5 minutes
		}

	}

	public void RandomGenerate()
	{
		if(auctionopen == true) {

			if (userbalance < bidnumber) {
				LogText.GetComponent<Text> ().text += "\n You don't have the balance!";
			} 
			else if (othersbid < bidnumber) {
				LogText.GetComponent<Text> ().text += "\n You Win the Bid!";
				//return Firebase server with bidding end signal
			}
			else {
				bidincrease = Random.Range (100000, 500000);
				bidnumber = bidnumber + bidincrease;
				othersbidincrease = Random.Range (-100000, 100000);
				othersbid = bidnumber + othersbidincrease;
				LogText.GetComponent<Text> ().text += "\n New bid: $" + bidnumber; 
				//CurrentPrice.GetComponent<Text> ().text = "$" + othersbid;
				//return the Firebase server with user's new bid number	
				auctionwait = true;
				auctiontime = Time.time;
			}

		}
		if(auctionopen == false) {
			LogText.GetComponent<Text> ().text += "\n Bidding not ready!";
		}
		if(auctionend == true) {
			LogText.GetComponent<Text> ().text += "\n Bidding Finished!";
			housevalue = Random.Range (500000, 1000000);
			userbalance = userbalance - bidnumber;
			userbalance = userbalance + (int) (housevalue * 0.9);
			LogText.GetComponent<Text> ().text += "\n Your Account Balance" + userbalance;
		}
	}
	public void Sellinghouse()
	{
		reserveprice = Random.Range (10000, 20000);
		housesupply = (int) (1/(reserveprice / 10000 - 1) * housesupply);
		//the higher reserve price goes, the less houses appear in the area.
		// if (housesupply < 1){} Triggers Hosing supply shortage news
		anverageprice = (int) ( (reserveprice / 10000 - 0.5) * anverageprice);
		//the higher reserve price goees, the higher anverage price rises.
		if (anverageprice > 1000000) {
			randomluxuryrate = Random.Range (0, 1);
			//if random price too high triggers luxury tax event
		}
		if (randomluxuryrate < 0.1) {
			luxuryrate = luxuryrate - randomluxuryrate;
		}
		//seller will be hit with a 10% chance of luxury tax up to 10%.
		if (housevalue < 1000000){
			agentfees = housevalue * 0.01;
		}
		//dynamic agent fees
		userbalance = userbalance + (int) ((housevalue - agentfees) * luxuryrate);


		}
		if(auctionopen == false) {
			LogText.GetComponent<Text> ().text += "\n Bidding not ready!";
		}
		if(auctionend == true) {
			LogText.GetComponent<Text> ().text += "\n Bidding Finished!";
			housevalue = Random.Range (500000, 1000000);
			userbalance = userbalance - bidnumber;
			userbalance = userbalance + (int) (housevalue * 0.9);
			LogText.GetComponent<Text> ().text += "\n Your Account Balance" + userbalance;
		}
	}

}
