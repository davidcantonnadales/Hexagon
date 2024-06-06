using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using SgLib;

#if EASY_MOBILE
using EasyMobile;
#endif

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public static bool firstLoad = true;

    [Header("Check to show menu when game starts")]
    public bool showMenuAtStart = false;

    [Header("Object References")]
    public GameManager gameManager;

    public GameObject gameoverPanle;
    public GameObject title;
    public GameObject mainCanvas;
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;

    public static int gameOverCount;
    public GameObject helpButton;


    public Text txtScore;
    public Text txtCombo;
    public Text txtComboText;
    public Text txtBestScore;
    public Text txtGameoverBestScore;
    public Text txtChangeRing;
    public Text txtNoMoreMoves;

    public GameObject avaleble;
    public GameObject notAvaleble;
    public Animation fivePlus;

    public GameObject instractopnText;
    public GameObject instractionArrow;

    public GameObject movieAd;
    public GameObject notAvalebaleAd;

    public Button btnChangeRing;
    public Button btnPause;
    public Button btnPlay;
    public Button btnReview;
    public Button btnSoundOn;
    public Button btnSoundOff;
    public Button btnVibrateOn;
    public Button btnVibrateOff;
    public GameObject gameOverButtons;
    public GameObject blackPanel;

    [Header("Premium Features Buttons")]
    public GameObject btnWatchAd;
    public GameObject leaderboardBtn;
    public GameObject leaderboardBtn2;
    //    public GameObject shareBtn;
    public GameObject removeAdsBtn;
    public GameObject restorePurchaseBtn;

    [Header("Sharing-Specific")]
    public GameObject shareUI;
    public Image sharedImage;

    public GameObject refillUnityAdButton;
    public string helpVideoLink = "";
    Animator scoreAnimator;
    public bool hasCheckedGameOver = false;

    public GameObject moreAppScreen;

    int t = 1;
    void Awake()
    {
        Instance = this;
    }

    public void OpenMoreAppScreen()
    {
        moreAppScreen.SetActive(true);
    }

    void OnEnable()
    {
        ScoreManager.ScoreUpdated += OnScoreUpdated;
    }

    void OnDisable()
    {
        ScoreManager.ScoreUpdated -= OnScoreUpdated;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine("InstractionEnumerator");
        scoreAnimator = txtScore.GetComponent<Animator>();
        pauseCanvas.SetActive(showMenuAtStart);

        // Enable or disable premium stuff
        bool enablePremium = gameManager.enablePremiumFeatures;
        leaderboardBtn.SetActive(enablePremium);
        leaderboardBtn2.SetActive(enablePremium);
        //        shareBtn.SetActive(enablePremium);
        removeAdsBtn.SetActive(enablePremium);
        restorePurchaseBtn.SetActive(enablePremium);

        // Hidden at start
        gameoverPanle.SetActive(false);
        shareUI.SetActive(false);
        settingsCanvas.SetActive(false);
        gameOverButtons.SetActive(false);
        btnWatchAd.SetActive(false);

        ShowGameplayUI();
        firstLoad = false;

        //UnityAdManager.instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsCanvas.activeInHierarchy && !pauseCanvas.activeInHierarchy)
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        txtScore.text = ScoreManager.Instance.Score.ToString();
        txtBestScore.text = ScoreManager.Instance.HighScore.ToString();
        txtGameoverBestScore.text  = ScoreManager.Instance.HighScore.ToString();
        txtChangeRing.text = CoinManager.Instance.Coins.ToString();
        //        if (!UnityAdManager.instance.IsLoaded())
        refillUnityAdButton.SetActive(true);
        if (gameManager.combo > 1)
        {
            ShowCombo();
        }
        else
        {
            HideCombo();
        }

        if (gameManager.gameOver && !hasCheckedGameOver)
        {
            hasCheckedGameOver = true;
            Invoke("ShowGameOverUI", 1f);
        }

        if (!GoogleMobileAdsDemoScript.Instance.rewardBasedVideo.IsLoaded())
        {
            if (!hasCheckedGameOver)
            {
                notAvaleble.SetActive(true);
                notAvalebaleAd.SetActive(true);
                avaleble.SetActive(false);
                movieAd.SetActive(false);
            }
             else
            {
                refillUnityAdButton.SetActive(false);
            }
        }
        else
        {
            if (!hasCheckedGameOver)
            {
                avaleble.SetActive(true);
                movieAd.SetActive(true);
                notAvaleble.SetActive(false);
                notAvalebaleAd.SetActive(false);
            }
            else
            {
                refillUnityAdButton.SetActive(false);
            }
           
        }

       

        UpdateMuteButtons();
        UpdateVibrateButtons();
    }

    void OnScoreUpdated(int newScore)
    {
        scoreAnimator.Play("NewScore");
    }

    public void HandleRestartButton()
    {
        StartCoroutine(CRRestart(0.2f));
    }

    public void HandlePlayButton()
    {
        if (!firstLoad)
        {
            StartCoroutine(CRRestart());
        }
        else
        {
            ShowGameplayUI();
            firstLoad = false;
        }
    }

    void ShowCombo()
    {
        txtCombo.gameObject.SetActive(true);
        txtCombo.text = "x" + gameManager.combo.ToString();
    }

    void HideCombo()
    {
        txtCombo.gameObject.SetActive(false);
    }


    public void ShowComboText(float waitTime)
    {
        StartCoroutine(ComboText(waitTime));
    }

    IEnumerator ComboText(float waitTime)
    {
        txtComboText.text = "combo x" + gameManager.combo.ToString();
        txtComboText.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        txtComboText.gameObject.SetActive(false);
    }

    public void HandleChangeRingButton()
    {
        if (CoinManager.Instance.Coins > 0 && gameManager.finishMoveRing)
        {
            gameManager.combo = 0;
            gameManager.finishMoveRing = false;
            StartCoroutine(gameManager.ChangeRing());
            CoinManager.Instance.AddCoins(-1);
#if UNITY_ANDROID
            // Analytics.Instance.LogEvent("Change Ring", "Click", "Change Ring");
#endif
        }
    }

    public void HandleReviewButton()
    {
        if (!firstLoad)
        {
            t = t * (-1);
            if (t < 0)
            {
                btnPlay.gameObject.SetActive(false);
                gameOverButtons.SetActive(false);
                blackPanel.gameObject.SetActive(false);
                txtNoMoreMoves.gameObject.SetActive(false);
            }
            else
            {
                btnPlay.gameObject.SetActive(true);
                gameOverButtons.SetActive(true);
                blackPanel.gameObject.SetActive(true);
                txtNoMoreMoves.gameObject.SetActive(true);
            }
        }
    }

    public void HandleSoundButton()
    {
        SoundManager.Instance.ToggleMute();
    }

    public void HandleVibrateButton()
    {
        SoundManager.Instance.ToggleVibrate();
    }

    public void ShowGameOverUI()
    {
        gameoverPanle.SetActive(true);

        // if (GameManager.instance.IsNoAdsPurchased)
        //   return;

        // refillUnityAdButton.SetActive(false);
        hasCheckedGameOver = true;
        gameOverCount++;
      
        // helpButton.SetActive(false);
        if (gameOverCount > 3)
        {
            // helpButton.SetActive(false);
        }
        ShowAdOnGameOver();
        gameOverButtons.SetActive(true);
        blackPanel.SetActive(true);
#if UNITY_ANDROID
        GoogleLeaderborad.Instance.OnAddScoreToLeaderBorad(ScoreManager.Instance.HighScore);
#endif
        btnPlay.gameObject.SetActive(true);
        btnReview.gameObject.SetActive(true);
        btnWatchAd.gameObject.SetActive(false);
        txtNoMoreMoves.gameObject.SetActive(true);

        btnChangeRing.gameObject.SetActive(false);
        btnPause.gameObject.SetActive(false);
        refillUnityAdButton.SetActive(false);

    }

    void ShowAdOnGameOver()
    {
        if (GameManager.instance.IsNoAdsPurchased)
            return;

        if (gameOverCount % 2 == 0)
        {
            /*if (UnityAdManager.instance.IsLoaded())
            {
                UnityAdManager.instance.ShowAd();
            }*/
            if (GoogleMobileAdsDemoScript.Instance.rewardBasedVideo.IsLoaded())
            {
                GoogleMobileAdsDemoScript.Instance.ShowRewardBasedVideo();
            }
            else
            {
                if (GoogleMobileAdsDemoScript.Instance.interstitial.IsLoaded())
                {
                    GoogleMobileAdsDemoScript.Instance.ShowInterstitial();
                }
                else
                {
                    GoogleMobileAdsDemoScript.Instance.RequestInterstitial();
                }

                GoogleMobileAdsDemoScript.Instance.RequestRewardBasedVideo();
                //                UnityAdManager.instance.Init();
            }
        }
        else
        {
            if (GoogleMobileAdsDemoScript.Instance.interstitial.IsLoaded())
            {
                GoogleMobileAdsDemoScript.Instance.ShowInterstitial();
            }
            else
            {
                /* if (UnityAdManager.instance.IsLoaded())
                 {
                     UnityAdManager.instance.ShowAd();
                 }
                 else
                 {
                     UnityAdManager.instance.Init();
                 }*/

                if (GoogleMobileAdsDemoScript.Instance.rewardBasedVideo.IsLoaded())
                {
                    GoogleMobileAdsDemoScript.Instance.ShowRewardBasedVideo();
                }
                else
                {
                    GoogleMobileAdsDemoScript.Instance.RequestRewardBasedVideo();
                }

                GoogleMobileAdsDemoScript.Instance.RequestInterstitial();
            }
        }
        /*if (gameOverCount % 2 == 0)
        {
            GoogleMobileAdsDemoScript.instance.ShowInterstitial();
        }
        else
        {
            UnityAdManager.instance.ShowAd();
        }*/
    }

    public void ShowGameplayUI()
    {
        gameoverPanle.SetActive(false);
        title.SetActive(false);
        GoogleMobileAdsDemoScript.Instance.RequestInterstitial();
        gameOverButtons.SetActive(false);
        blackPanel.SetActive(false);
        btnPlay.gameObject.SetActive(false);
        btnReview.gameObject.SetActive(false);
        txtNoMoreMoves.gameObject.SetActive(false);
        btnWatchAd.gameObject.SetActive(false);
        txtComboText.gameObject.SetActive(false);
        refillUnityAdButton.SetActive(true);
        // helpButton.SetActive(true);
        txtCombo.gameObject.SetActive(true);
        txtScore.gameObject.SetActive(true);
        btnChangeRing.gameObject.SetActive(true);
        btnPause.gameObject.SetActive(true);
    }

    public void OpenHelpVideo()
    {
#if UNITY_ANDROID
        //Analytics.Instance.LogEvent("Video Help", "Click", "Help");
#endif
        Application.OpenURL(helpVideoLink);
    }

    public void ShowSettingsUI()
    {

        txtNoMoreMoves.enabled = false;
     // txtBestScore.enabled = false;
        btnReview.enabled = false;


        gameOverButtons.SetActive(false);
        gameoverPanle.SetActive(false);
        pauseCanvas.SetActive(false);
        //        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
#if UNITY_ANDROID
        //Analytics.Instance.LogScreen("Settings");
#endif
    }

    public void HideSettingsUI()
    {
       // gameoverPanle.SetActive(true);
        pauseCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        //        pauseCanvas.SetActive(true);
    }

    public void ShowWatchAdButton()
    {
        //bntWatchAdMob.gameObject.SetActive(true);
        btnWatchAd.gameObject.SetActive(true);
        //btnChangeRing.gameObject.SetActive(false);
    }

    public void ShowLeaderboardUI()
    {
#if UNITY_ANDROID
        //  Analytics.Instance.LogScreen("Leaderboard");
#endif

#if UNITY_ANDROID
        GoogleLeaderborad.Instance.OnShowLeaderBoard();
#endif

#if UNITY_IOS
        LeaderboardView.Instance.OnShowLeaderboard();
#endif
        /*#if EASY_MOBILE
                if (GameServiceManager.IsInitialized())
                {
                    GameServiceManager.ShowLeaderboardUI();
                }
                else
                {
#if UNITY_IOS
                MobileNativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
#elif UNITY_ANDROID
                    GameServiceManager.Init();
#endif
                }
#endif*/
    }

    public void PurchaseRemoveAds()
    {
        Debug.Log("Enter");
        IAPManager.instace.PurchaseNoAds();
#if UNITY_ANDROID
        //Analytics.Instance.LogEvent("No Ads Purchase", "Click", "IAP");
#endif
#if EASY_MOBILE
        InAppPurchaser.Instance.Purchase(InAppPurchaser.Instance.removeAds);
#endif
    }

    public void RestorePurchase()
    {
        IAPManager.instace.RestoreAllPurchases();
#if UNITY_ANDROID
        // Analytics.Instance.LogEvent("Restore Purchases", "Click", "IAP");
#endif
#if EASY_MOBILE
        InAppPurchaser.Instance.RestorePurchase();
#endif
    }

    public void ShowShareUI()
    {
#if EASY_MOBILE
        Texture2D texture = ScreenshotSharer.Instance.GetScreenshotTexture();

        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Transform imgTf = sharedImage.transform;
            Image imgComp = imgTf.GetComponent<Image>();
            float scaleFactor = imgTf.GetComponent<RectTransform>().rect.width / sprite.rect.width;
            imgComp.sprite = sprite;
            imgComp.SetNativeSize();
            imgTf.localScale = imgTf.localScale * scaleFactor;

            shareUI.SetActive(true);
        }
#endif
    }

    public void HideShareUI()
    {
        shareUI.SetActive(false);
    }

    public void ShareScreenshot()
    {
#if EASY_MOBILE
        shareUI.SetActive(false);
        ScreenshotSharer.Instance.ShareScreenshot();
#endif
    }

    public void RateApp()
    {
        Utilities.RateApp();
    }

    public void OpenTwitterPage()
    {
        Utilities.OpenTwitterPage();
    }

    public void OpenFacebookPage()
    {
        Utilities.OpenFacebookPage();
    }

    public void ContactUs()
    {
        Utilities.ContactUs();
    }

    public void ButtonClickSound()
    {
        Utilities.ButtonClickSound();
    }

    void UpdateMuteButtons()
    {
        if (SoundManager.Instance.IsMuted())
        {
            btnSoundOn.gameObject.SetActive(false);
            btnSoundOff.gameObject.SetActive(true);
        }
        else
        {
            btnSoundOn.gameObject.SetActive(true);
            btnSoundOff.gameObject.SetActive(false);
        }
    }
    void UpdateVibrateButtons()
    {
        if (SoundManager.Instance.IsVibrateOn())
        {
            btnVibrateOn.gameObject.SetActive(false);
            btnVibrateOff.gameObject.SetActive(true);
        }
        else
        {
            btnVibrateOn.gameObject.SetActive(true);
            btnVibrateOff.gameObject.SetActive(false);
        }
    }

    IEnumerator CRRestart(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void HandlePlayButtonOnPauseCanvas()
    {
        pauseCanvas.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        gameManager.isPaused = false;
    }

    public void HandlePauseButton()
    {
        gameManager.isPaused = true;
        mainCanvas.gameObject.SetActive(false);
        pauseCanvas.transform.Find("BestScore").GetComponent<Text>().text = ScoreManager.Instance.HighScore.ToString();
        pauseCanvas.gameObject.SetActive(true);

    }

    public void CheckAndShowWatchAdOption()
    {
        if (CoinManager.Instance.Coins == 0 && !btnWatchAd.activeSelf)
        {
            if (gameManager.enablePremiumFeatures)
            {
                StartCoroutine(CRShowWatchAdOption(0.5f));
            }
            else
            {
                ShowChangeRingButton();
            }
        }
        else if (CoinManager.Instance.Coins > 0 && !btnChangeRing.gameObject.activeSelf)
        {
            ShowChangeRingButton();
        }
    }

    IEnumerator InstractionEnumerator()
    {
        yield return new WaitForSeconds(4f);
        instractionArrow.SetActive(false);
        instractopnText.SetActive(false);
    }

    public void ShowChangeRingButton()
    {
        btnChangeRing.gameObject.SetActive(true);
        btnWatchAd.SetActive(false);
    }

    IEnumerator CRShowWatchAdOption(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        if (CoinManager.Instance.Coins == 0)
        {

            // Only show "watch ad" button if a rewarded ad is loaded and premium features are enabled
#if EASY_MOBILE
            if (gameManager.enablePremiumFeatures && AdDisplayer.Instance.CanShowRewardedAd() && AdDisplayer.Instance.watchAdToRefillRingSwap)
            {
                btnWatchAd.SetActive(true);
                btnChangeRing.gameObject.SetActive(false);
            }
            else
            {
#if !UNITY_EDITOR
                ShowChangeRingButton(); 
#else
                if (gameManager.enablePremiumFeatures)
                {
                    btnWatchAd.SetActive(true);   // for testing in the editor
                    btnChangeRing.gameObject.SetActive(false);
                }
#endif
            }
#elif UNITY_EDITOR
            if (gameManager.enablePremiumFeatures)
            {
                // btnWatchAd.SetActive(true);
                // for testing in the editor
               // btnChangeRing.gameObject.SetActive(false);
            }
#endif
        }
    }

    public void WatchRewardedAd()
    {
        //        if (UnityAdManager.instance.IsLoaded())
        if (GoogleMobileAdsDemoScript.Instance.rewardBasedVideo.IsLoaded())
        {
#if UNITY_ANDROID
            //            Analytics.Instance.LogEvent("Rewarded Ad", "Click", "UntiyAd");
            // Analytics.Instance.LogEvent("Rewarded Ad", "Click", "Admob");
#endif

            ShowChangeRingButton();
            //btnWatchAd.SetActive(true);
            GoogleMobileAdsDemoScript.Instance.ShowRewardBasedVideo();
            //            UnityAdManager.instance.ShowRewardAd();
        }
        else
        {
            GoogleMobileAdsDemoScript.Instance.RequestRewardBasedVideo();
        }
#if UNITY_EDITOR
        if (gameManager.enablePremiumFeatures)
        {
            // Give the award right away for testing in the editor
            // Show the change ring button
            ShowChangeRingButton();
            RewardAfterWatchAd(5);  // for testing in the editor

        }
#elif EASY_MOBILE
        // Show the change ring button
        ShowChangeRingButton();

        AdDisplayer.CompleteRewardedAd += OnCompleteRewardedAd;
        AdDisplayer.Instance.ShowRewardedAd();
#endif
    }

    void OnCompleteRewardedAd()
    {
#if EASY_MOBILE
        // Unsubscribe
        AdDisplayer.CompleteRewardedAd -= OnCompleteRewardedAd;
        RewardAfterWatchAd(AdDisplayer.Instance.rewardedRingSwaps);
#endif
    }

    public void RewardAfterWatchAd(int amount)
    {
        // Give the award!
        CoinManager.Instance.AddCoins(amount);
        fivePlus.GetComponent<Animation>().Play();
    }
}
