using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * House script contains all the information for a single house, image, location, price will be used for displaying in the portfolio and during an auction
 */

public class House {

    private string uid;
    private Sprite houseImage;
    private int streetNo;
    private string address;
    private string postcode;

    public House(string id, int sn, string a, string pc) {
        uid = id;
        streetNo = sn;
        address = a;
        postcode = pc;
    }

    public Dictionary<string, System.Object> ToDictionary() {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["streetNo"] = streetNo;
        result["address"] = address;
        result["postcode"] = postcode;

        return result;
    }

    House(Sprite image) {
        HouseImage1 = image;
    }

    public Sprite HouseImage {
        get {
            return HouseImage1;
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

    public Sprite HouseImage1 {
        get {
            return houseImage;
        }

        set {
            houseImage = value;
        }
    }

    public int StreetNo {
        get {
            return streetNo;
        }

        set {
            streetNo = value;
        }
    }

    public string Address {
        get {
            return address;
        }

        set {
            address = value;
        }
    }

    public string Postcode {
        get {
            return postcode;
        }

        set {
            postcode = value;
        }
    }
}
