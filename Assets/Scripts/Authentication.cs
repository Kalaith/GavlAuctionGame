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

    public Text errorLog;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // Use this for initialization
    void Start () {
        errorLog.text = "Starting Authentication\n";
        menu = (LoadMenu)GameObject.Find("Setup").GetComponent(typeof(LoadMenu));
        menu.LoadGameLogin();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                InitializeFirebase();
            } else {
                errorLog.text = (
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        errorLog.text = "Firebase Init complete \n";
    }

    // Update is called once per frame
    void Update () {
		
	}

    void InitializeFirebase() {

        errorLog.text = "a \r\n";
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        errorLog.text = "b \r\n";
        auth.StateChanged += AuthStateChanged;
        errorLog.text = "c \r\n";
        AuthStateChanged(this, null);
        errorLog.text = "d \r\n";
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                errorLog.text += ("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                errorLog.text += ("Signed in " + user.UserId);
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
        errorLog.text = "Start create user \n";
        //TODO Clean email/password
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                errorLog.text += "CreateUserWithEmailAndPasswordAsync was canceled.";
                return;
            }
            if (task.IsFaulted) {
                errorLog.text += "CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            errorLog.text += "Firebase user created successfully: " + newUser.DisplayName;
            p = new Player(newUser.UserId, newUser.DisplayName, email.text, null);

            menu.LoadGameHome();
        });
    }

    public void loginUser() {
        //TODO Clean email/password
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            errorLog.text = "Begin login \r\n";
            if (task.IsCanceled) {
                errorLog.text += ("SignInWithEmailAndPasswordAsync was canceled. \r\n");
                return;
            }
            if (task.IsFaulted) {
                errorLog.text += ("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception + "\r\n");
                return;
            }

            FirebaseUser newUser = task.Result;
            errorLog.text += "User signed in successfully "+ newUser.DisplayName+ "\r\n";

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
