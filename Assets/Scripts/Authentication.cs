using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Auth;

public class Authentication : MonoBehaviour {
    FirebaseAuth auth;
    FirebaseUser user;

    Player p;

    public Text email;
    public Text password;

    // Use this for initialization
    void Start () {
        InitializeFirebase();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeFirebase() {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
                p = new Player(user.UserId, user.DisplayName, user.Email, user.PhotoUrl);

            }
        }
    }

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void logoutUser() {
        auth.SignOut();
    }

    public void createUser() {

        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

        });
    }

    public void loginUser() {

        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });

    }

}
