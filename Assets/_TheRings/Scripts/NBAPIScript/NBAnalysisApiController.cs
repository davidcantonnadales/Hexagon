using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

using UnityEngine.UI;

public class NBAnalysisApiController : MonoBehaviour
{
    public static NBAnalysisApiController instance;
#region Public_Variables
    public AppInfoData appInfo = new AppInfoData();
    public List<Sprite> appIcon = new List<Sprite>();

    public Button moreAppButton;
#endregion

#region Private_Variables

    string UserID
    {
        get { return PlayerPrefs.GetString("UserID", string.Empty); }
        set
        {
            if (!PlayerPrefs.HasKey("UserID"))
                PlayerPrefs.GetString("UserID", string.Empty);
            PlayerPrefs.SetString("UserID", value);
        }
    }
#endregion

#region Events
#endregion

#region Unity_CallBacks

    void Awake()
    {
        instance = this;
    }
    void OnEnable() { }
    // Use this for initialization
    void Start()
    {
        CreateUser();
        StartCoroutine("GetAppInfo");
    }
    void OnDisable() { }
#endregion

#region Private_Methods
#endregion

#region Public_Methods

    public void CreateUser()
    {
        //create-user
        if (string.IsNullOrEmpty(UserID))
            StartCoroutine(UserCreateEnumerator());
    }
#endregion

#region UI_Calls
#endregion

#region Coroutines

    IEnumerator UserCreateEnumerator()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("app_id", 24);
        wwwForm.AddField("country", System.Globalization.RegionInfo.CurrentRegion.ToString());
        wwwForm.AddField("device_type", SystemInfo.deviceModel);
        wwwForm.AddField("os_version", SystemInfo.operatingSystem);
        wwwForm.AddField("app_version", Application.version);
        wwwForm.AddField("device_id", SystemInfo.deviceUniqueIdentifier);

        WWW www = new WWW("http://api.9brainz.store/api/users_create", wwwForm);
        yield return www;

        if (www.error == null)
        {
            NB_UserCreateResponse response = JsonUtility.FromJson<NB_UserCreateResponse>(www.text);
            if (response != null && response.success)
            {
                UserID = response.User.id.ToString();
            }
        }
    }

    IEnumerator GetAppInfo()
    {
        moreAppButton.interactable = false;

        WWWForm dataForm = new WWWForm();
        WWW appInfoData = new WWW("http://api.9brainz.store/api/moreApp?app_platform_id=2");
        //#if UNITY_ANDROID
        /*#else
                WWW appInfoData = new WWW ("http://api.9brainz.store/api/moreApp?app_platform_id=1");
        */
        yield return appInfoData;

        appInfo = JsonUtility.FromJson<AppInfoData>(appInfoData.text);

        if (appInfo.success)
        {
            /* for (int i = 0; i < appInfo.apps.Count; i++) {
                 SpriteRenderer s = new SpriteRenderer ();
                 appIcon.Add (s);
             }*/

            int randomIndex = UnityEngine.Random.Range(0, appInfo.apps.Count);

            if (randomIndex < appInfo.apps.Count)
            {

                Debug.Log(randomIndex);
                PopupInfo.Instance.SetInfoPopup(appInfo.apps[randomIndex].title_of_ad, appInfo.apps[randomIndex].messge_of_ad);
                PopupInfo.Instance.URL = appInfo.apps[randomIndex].link;
                PopupInfo.Instance.PopupEnableDisable(true);
            }
        }
        moreAppButton.interactable = true;

    }

    IEnumerator GetAppIcon(string url)
    {
        WWW www = new WWW(url);

        yield return www;
        Debug.Log(www.text);
        Sprite s;
        var texture = new Texture2D(1024, 1024);
        texture = www.texture;
        s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100);
        appIcon.Add(s);
        // PosterManager.Instance.SetLogo (s, 2030, 1608);
    }
    IEnumerator WaitIcon(string URl)
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GetAppIcon(URl));
    }
#endregion

#region Custom_CallBacks
#endregion
}

public class NB_User
{
    public int id { get; set; }
    /*public string device_id { get; set; }
    public string country { get; set; }
    public string device_type { get; set; }
    public string os_version { get; set; }
    public string app_version { get; set; }
    public string latest_version { get; set; }
    public bool is_force_update { get; set; }
    public bool is_full_access { get; set; }
    public bool is_purchase_ads { get; set; }
    public bool is_only_banner { get; set; }
    public bool is_purchase_watermark { get; set; }
    public bool is_purchase_unlimited { get; set; }
    public bool is_purchase_subscription { get; set; }
    public string last_date_of_subscription { get; set; }*/
}

public class NB_UserCreateResponse
{
    public bool success { get; set; }
    public NB_User User { get; set; }
}

[Serializable]
public class App
{
    public int id;
    public string name;
    public string latest_version;
    public bool is_force_update;
    public string title_of_ad;
    public string messge_of_ad;
    public string link;
    public string contact_email;
    public string share_format;
    public string contact_format;
    public string developer_site;
    public string developer_name;
    public string developer_apps;
    public string generated_in_app;
    public bool is_only_banner;
    public string icon_url;
}

[Serializable]
public class AppInfoData
{
    public bool success;
    public List<App> apps = new List<App>();
}