using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{
	public GameObject username;
	public GameObject email;
	public GameObject password;
	public GameObject confPassword;
	// Use this for initialization

	public void LoginBtn(){

		print("Sign Up Successful");
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown<InputField> (KeyCode.Tab)) {
			if (username.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
		}
		if(Input.GetKeyDown(KeyCode.Return)){
			if(Password != ""&& Password !=""){
				LoginButton ();
			}
		}
		Username = username.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
	}
}