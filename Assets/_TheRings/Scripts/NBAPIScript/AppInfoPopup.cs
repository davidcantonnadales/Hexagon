using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AppInfoPopup : MonoBehaviour
{

    public static AppInfoPopup Instance;

    [SerializeField]
    public Texture2D texture2D;

    #region Public_Variables
    public Text devloperNameText;
    public GameObject appInfoPopupScreen;
    public int id;
    public string name;
    public string title_of_ad;
    public string messge_of_ad;
    public string link;
    public string contact_email;
    public string share_format;
    public string contact_format;
    public string developer_site;
    public string developer_name;
    public string developer_apps;

    public NativeShare share = new NativeShare();
    #endregion

    #region Private_Variables
    #endregion

    #region Events
    #endregion

    #region Unity_CallBacks
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        //devloperNameText.Text = NBAnalysisApiController.instance.appInfo.apps[]
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {

    }
    #endregion

    #region Private_Methods
    #endregion

    #region Public_Methods
    public void SetInfo(int tempid, string tempname, string temptitle_of_ad, string tempmessge_of_ad,
        string templink, string tempcontact_email, string tempshare_format, string tempcontact_format,
        string tempdeveloper_site, string tempdeveloper_name, string tempdeveloper_apps)
    {
        id = tempid;
        name = tempname;
        title_of_ad = temptitle_of_ad;
        messge_of_ad = tempmessge_of_ad;
        link = templink;
        contact_email = tempcontact_email;
        share_format = tempshare_format;
        contact_format = tempcontact_format;
        developer_site = tempdeveloper_site;
        developer_name = tempdeveloper_name;
        developer_apps = tempdeveloper_apps;

        devloperNameText.text = "Developer Name : " + developer_name;
        appInfoPopupScreen.SetActive(true);
    }

    public void GotoStoreButtonClick()
    {
        Application.OpenURL(link);
    }

    public void CancleButtonClick()
    {
        appInfoPopupScreen.SetActive(false);
    }

    public void ShareThisApp()
    {

        //share.AddFile (link + ".png", share_format).SetText (share_format).Share ();
        //GeneralSharing.Instance.OnShareSimpleText (share_format);
        string s = share_format + " : " + link;
        Debug.Log(s);
        share.SetText(s);
        share.Share();
    }
    public void SendEmail()
    {
        string email = contact_email;
        string subject = MyEscapeURL(title_of_ad);
        string body = MyEscapeURL(share_format);
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    #endregion

    #region Coroutines
    #endregion

    #region Custom_CallBacks
    #endregion
}