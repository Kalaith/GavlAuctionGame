using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// A collection of classes to support the various types of objects, not worth their own file.
/// </summary>

public class AuctionListItem {

    public string house_uid;
    public DateTime date;
    public DateTime time;
    public int bidders;
    public const int max_bidders = 6;
    public double current_price;

    /// <summary>
    /// A new auction, takes in a house object, a time and how many bidders
    /// </summary>
    /// <param name="u">House uid for Auction</param>
    /// <param name="d">Date and time of the auction</param>
    /// <param name="b">How many bidders, default 0</param>
    public AuctionListItem(string u, DateTime d, int b = 0) {
        house_uid = u;
        date = DateTime.Parse(d.ToShortDateString());
        time = DateTime.Parse(d.ToShortTimeString());
        bidders = b;
        current_price = 0;
    }

    public AuctionListItem(IDictionary ali) {
        house_uid = (string)ali["house_uid"];
        date = Convert.ToDateTime(ali["date"]);
        time = Convert.ToDateTime(ali["time"]);
        bidders = Convert.ToInt32(ali["bidders"]);
        current_price = Convert.ToDouble(ali["current_price"]);
    }

    public override string ToString() {
        return "Auction at " + time.ToShortTimeString() + " on the " + date.ToShortDateString() + " Current Bidders: "+bidders;

    }

    public Dictionary<string, System.Object> ToDictionary() {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["house_uid"] = house_uid;
        result["date"] = date.ToShortDateString();
        result["time"] = date.ToShortTimeString();
        result["bidders"] = bidders;
        result["current_price"] = current_price;

        return result;
    }
}