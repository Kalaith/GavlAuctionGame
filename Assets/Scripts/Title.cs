using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {

	public GameObject Button1;
	// Use this for initialization
	void Start () {
		Button1 = transform.FindChild ("Back").gameObject;
		UIEventListener.Get (Button1).onClick += TestButton;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TestButton(GameObject go){
		Debug.Log ("Preparing for you" + go.HowToPlay);
	}
}
