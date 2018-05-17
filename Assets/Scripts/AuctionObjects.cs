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
    public int bidders;
    public const int max_bidders = 6;
    public double current_price;
    public bool full;
    public bool closed;

    /// <summary>
    /// A new auction, takes in a house object, a time and how many bidders
    /// </summary>
    /// <param name="u">House uid for Auction</param>
    /// <param name="d">Date and time of the auction</param>
    /// <param name="b">How many bidders, default 0</param>
    public AuctionListItem(string u, DateTime d, int b = 0, double p = 0) {
        house_uid = u;
        date = d;
        bidders = b;
        current_price = p;
    }

    // Create an auction event from a dictionary
    public AuctionListItem(IDictionary ali) {

        house_uid = (string)ali["house_uid"];

        System.DateTime auction_time;
        auction_time = Convert.ToDateTime(ali["time"]);
        date = Convert.ToDateTime(ali["date"]);
        date = date.Add(auction_time.TimeOfDay);

        current_price = Convert.ToDouble(ali["current_price"]);

    }

    public override string ToString() {
        return "Auction at " + date.ToShortTimeString() + " on the " + date.ToShortDateString() + " Current Bidders: "+bidders;

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