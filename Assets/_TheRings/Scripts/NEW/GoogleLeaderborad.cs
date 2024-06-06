#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using SgLib;
using UnityEngine.SocialPlatforms;

public class GoogleLeaderborad : MonoBehaviour
{

    #region PUBLIC_VAR
    public string leaderboard;
    public static GoogleLeaderborad Instance;
    private bool mAuthenticating;
    public bool Authenticated;
    static ILeaderboard HighScoreLeaderboard;
    #endregion

    #region DEFAULT_UNITY_CALLBACKS

    void Start()
    {
        LogIn();
    }
    #endregion

    #region BUTTON_CALLBACKS

    void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Login In Into Your Google+ Account
    /// </summary>
    public void LogIn()
    {
        if (Authenticated || mAuthenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        /*// Enable/disable logs on the PlayGamesPlatform
        PlayGamesPlatform.DebugLogEnabled = GameConsts.PlayGamesDebugLogsEnabled;*/

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);

        // Activate the Play Games platform. This will make it the default
        // implementation of Social.Active
        PlayGamesPlatform.Activate();

        // Set the default leaderboard for the leaderboards UI
        ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GPGSIds.leaderboard_hexa_color_ring_puzzle);

        // Sign in to Google Play Games
        mAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            mAuthenticating = false;
            Authenticated = success;
            if (success)
            {
                GetHighscoreFromGPGS();
                // if we signed in successfully, load data from cloud
                Debug.Log("Login successful!");
            }
            else
            {
                // no need to show error message (error messages are shown automatically
                // by plugin)
                Debug.LogWarning("Failed to sign in with Google Play Games.");
            }
        });
    }

    /* void Update()
     {
         /*if (Input.GetKeyDown(KeyCode.Escape))
         {
             UIManager.Instance.leaderborad.SetActive(false);
         }#1#
     }*/
    /// <summary>
    /// Shows All Available Leaderborad
    /// </summary>
    public void OnShowLeaderBoard()
    {
        if (Authenticated)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_hexa_color_ring_puzzle);
        }
        else
        {
            LogIn();
        }
    }

    /// <summary>
    /// Adds Score To leader board
    /// </summary>
    public void OnAddScoreToLeaderBorad(long score)
    {
        //Debug.Log(ScoreManager.Instance.highScore);
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_hexa_color_ring_puzzle, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");
                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }
    }

    /// <summary>
    /// On Logout of your Google+ Account
    /// </summary>
    public void OnLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    void GetHighscoreFromGPGS()
    {
        string[] UserFilter = { PlayGamesPlatform.Instance.localUser.id }; // Get the local player username
        HighScoreLeaderboard = PlayGamesPlatform.Instance.CreateLeaderboard(); // initiate 
        HighScoreLeaderboard.SetUserFilter(UserFilter); // Filter out other players
        PlayGamesPlatform.Instance.LoadScores(HighScoreLeaderboard, (RWResult =>
        {
            if (RWResult)
            {
                updateHighScoreOnDevice();
            }
        }));
    }

    private void updateHighScoreOnDevice()
    {
        // Update MyHighScore variable with data pulled from the leaderboard.
        Debug.Log("High Score : " + (int)HighScoreLeaderboard.localUserScore.value);
        ScoreManager.Instance.UpdateHighScore((int)HighScoreLeaderboard.localUserScore.value);
    }
    #endregion

}
#endif