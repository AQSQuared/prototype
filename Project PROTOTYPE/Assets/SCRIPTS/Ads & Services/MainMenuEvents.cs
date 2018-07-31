using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{

    private Text signInButtonText;
    private Text authStatus;

    public void Start()
    {
        GameObject startButton = GameObject.Find("ModeButton");
        EventSystem.current.firstSelectedGameObject = startButton;

        // Get object instances
        signInButtonText = GameObject.Find("signInButton").GetComponentInChildren<Text>();
        authStatus = GameObject.Find("authStatus").GetComponent<Text>();

        //  ADD THIS CODE BETWEEN THESE COMMENTS

        // Create client configuration
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
        SignIn();

        // END THE CODE TO PASTE INTO START
    }

    public void SignIn()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            // Sign out of play games
            PlayGamesPlatform.Instance.SignOut();

            // Reset UI
            signInButtonText.text = "CONNECT";
            authStatus.text = "";
        }
    }

    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("() Signed in!");

            // Change sign-in button text
            signInButtonText.text = "DISCONNECT";

            // Show the user's name
            authStatus.text = "connected as " + Social.localUser.userName;
        }
        else
        {
            Debug.Log("() Sign-in failed...");

            // Show failure message
            signInButtonText.text = "CONNECT";
            authStatus.text = "connection failed";
        }
    }

    // ...
}

