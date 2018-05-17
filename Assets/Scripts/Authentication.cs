using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Auth;

public class Authentication : MonoBehaviour {
    FirebaseAuth auth;
    FirebaseUser user;

    public Player p;

    public InputField email;
    public InputField password;
    private LoadMenu menu;
    
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // Use this for initialization
    void Start () {
        menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
        menu.LoadGameLogin();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                InitializeFirebase();
            } else {
            }
        });

    }

    // Update is called once per frame
    void Update () {
		
	}

    void InitializeFirebase() {
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                // signed out successfully
            }
            user = auth.CurrentUser;
            if (signedIn) {
                // signed in successfully
            }
        }
    }

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
        p = null;
    }

    public void logoutUser() {
        auth.SignOut();
        p = null;
    }

    public void createUser() {
        //TODO Clean email/password
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                return;
            }
            if (task.IsFaulted) {
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            p = new Player(newUser.UserId, newUser.DisplayName, email.text, null);

            menu.LoadGameHome();
        });
    }

    public void loginUser() {
        //TODO Clean email/password
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                return;
            }
            if (task.IsFaulted) {
                return;
            }

            FirebaseUser newUser = task.Result;

            p = new Player(newUser.UserId, newUser.DisplayName, email.text, null);

            menu.LoadGameHome();
        });

    }

    public string getPlayerUID() {
        return p.Uid;
    }
    public string getPlayerName() {
        return p.DisplayName;
    }

}
