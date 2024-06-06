using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{

    public static GoogleMobileAdsDemoScript Instance;

    public BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardBasedVideoAd rewardBasedVideo;
    private float deltaTime = 0.0f;
    private static string outputMessage = string.Empty;

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

#if UNITY_ANDROID
        string appId = "ca-app-pub-8989292844401169~4813866571";
#elif UNITY_IPHONE
		string appId = "ca-app-pub-8989292844401169~4813866571";
#else
        string appId = "unexpected_platform";
#endif

        //MobileAds.SetiOSAppPauseOnBackground (true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

        if (!GameManager.instance.IsNoAdsPurchased)
        {
            RequestBanner();
            RequestInterstitial();
            ShowInterstitial();
            RequestRewardBasedVideo();
        }
    }

    public void Update()
    {
        // Calculate simple moving average for time to render screen. 0.1 factor used as smoothing
        // value.
        this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        /* 	return new AdRequest.Builder ()
				.AddTestDevice (AdRequest.TestDeviceSimulator)
				.AddTestDevice ("0123456789ABCDEF0123456789ABCDEF")
				.AddKeyword ("game")
				.SetGender (Gender.Male)
				.SetBirthday (new DateTime (1985, 1, 1))
				.TagForChildDirectedTreatment (false)
				.AddExtra ("color_bg", "9B30FF")
				.Build ();*/
        AdRequest request = new AdRequest.Builder().Build();
        return request;
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }

    public void RequestBanner()
    {

        if (GameManager.instance.IsNoAdsPurchased)
            return;

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-8989292844401169/9388192947";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-8989292844401169/9388192947";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {

        if (GameManager.instance.IsNoAdsPurchased)
            return;

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-8989292844401169/9491478185";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-8989292844401169/9491478185";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardBasedVideo()
    {
        if (GameManager.instance.IsNoAdsPurchased)
            return;

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-8989292844401169/5939245989";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-8989292844401169/5939245989";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            MonoBehaviour.print("Interstitial is not ready yet");
        }
    }

    public void ShowRewardBasedVideo()
    {
        if (GameManager.instance.IsNoAdsPurchased)
            return;

        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            MonoBehaviour.print("Reward based video ad is not ready yet");
        }
    }

    public void DestroyAd()
    {
        this.interstitial.Destroy();
        this.bannerView.Destroy();
    }
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion
}