using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionListEntry : MonoBehaviour {

    [SerializeField]
    private Text entry;

    public void SetText(string uid) {
        entry.text = uid;
    }

    public void OnClick() {

    }


}
