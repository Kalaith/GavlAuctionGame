using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
		/*if (Input.GetKeyDown<InputField> (KeyCode.Tab)) {
			if (username.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
		}
		if(Input.GetKeyDown(KeyCode.Return)){
			if(password != ""&& password !=""){
				LoginButton ();
			}
		}
		username = username.GetComponent<InputField> ().text;
		password = password.GetComponent<InputField> ().text;*/
	}
}