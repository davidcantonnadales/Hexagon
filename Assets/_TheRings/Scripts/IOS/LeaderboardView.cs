using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;




public class LeaderboardView : MonoBehaviour
{

    public static LeaderboardView Instance;
    #region PRIVATE_VARIABLES

    string leaderBoardID = "digital572";

    #endregion

    #region BUTTON_EVENT_HANDLER

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnLogin();
    }

    /// <summary>
    /// Raises the login event.
    /// </summary>
    /// <param name="id">Identifier.</param>

    public void OnLogin()
    {
        IOSLeaderboardManager.AuthenticateToGameCenter();
    }

    /// <summary>
    /// Raises the show leaderboard event.
    /// </summary>

    public void OnShowLeaderboard()
    {
        IOSLeaderboardManager.ShowLeaderboard("High Score");

    }

    /// <summary>
    /// Raises the post score event.
    /// </summary>

    public void OnPostScore(int score)
    {
        IOSLeaderboardManager.ReportScore(score, leaderBoardID);
    }
    #endregion
}
