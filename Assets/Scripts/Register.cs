using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Register : MonoBehaviour {

	public GameObject username;
	public GameObject email;
	public GameObject password;
	public GameObject confPassword;
	private string Username;
	private string Email;
	private string Password;
	private string ConfPassword;
	private string form;
	private bool EmailValid = false;

	// Use this for initialization
	void Start () {
		
	}
	public void SignupButton(){
		print("Sign Up Successful");
	}
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown<InputField> (KeyCode.Tab)) {
			if (username.GetComponent<InputField> ().isFocused) {
				email.GetComponent<InputField> ().Select ();
			}
			if (email.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
			if (password.GetComponent<InputField> ().isFocused) {
				confPassword.GetComponent<InputField> ().Select ();
			}
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			if(Password != ""&& Email !=""&& Password !=""&& ConfPassword !=""){
				SignupButton ();
			}
		}

		Username = username.GetComponent<InputField> ().text;
		Email = email.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
		ConfPassword = confPassword.GetComponent<InputField> ().text;
        */
	}
}
