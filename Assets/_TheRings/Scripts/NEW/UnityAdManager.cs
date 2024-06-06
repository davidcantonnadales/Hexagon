using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using SgLib;

public class UnityAdManager : MonoBehaviour
{
    #region Public_vars

    public static UnityAdManager instance;
    string gameId = null;

    [SerializeField]
    private string androidGameId = "", iosGameId = "";

    [SerializeField]
    private bool testMode;

    private static bool _firstShow;
    #endregion

    #region Unity_CallBacks
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_IOS
        gameId = iosGameId;
#endif

        Init();
    }

    void Update()
    {
       /* if (Advertisement.IsReady() && !_firstShow)
        {
            _firstShow = true;
            ShowAd();
        }*/
    }
    #endregion

    #region Public_Methods

    public void Init()
    {
        /*if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode);
        }*/
    }

    public void ShowRewardAd()
    {
     /*   ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Init();
        }*/
    }
    public void ShowAd()
    {
        if (GameManager.instance.IsNoAdsPurchased)
        {
            return;
        }

       /* ShowOptions options = new ShowOptions();

        if (Advertisement.IsReady())
        {
            Advertisement.Show("video", options);
        }
        else
        {
            Init();
        }*/
    }
    public bool IsLoaded()
    {
        return false;
      //  return Advertisement.IsReady();

    }
    #endregion

    #region Private_Methods

   /* private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                UIManager.Instance.RewardAfterWatchAd(5);
                Debug.LogWarning("Video was finished.");
                Init();
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }*/

    #endregion

    #region Coroutine
    #endregion

    }