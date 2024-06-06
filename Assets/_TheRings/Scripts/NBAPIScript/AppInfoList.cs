using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AppInfoList : MonoBehaviour {

	#region Public_Variables
	public int id;
	public Image icon;
	public Text gamenameText;
	public Button gotoAppButton;
	public string URL;

	public AppInfoPopup appInfoPopup;
	#endregion

	#region Private_Variables
	#endregion

	#region Events
	#endregion

	#region Unity_CallBacks
	void Awake () {

	}

	void OnEnable () { }

	// Use this for initialization
	void Start () {
		appInfoPopup = FindObjectOfType<AppInfoPopup> ();
		gotoAppButton.onClick.AddListener (GotoButtonClick);

	}

	// Update is called once per frame
	void Update () {

	}

	void OnDisable () {

	}
	#endregion

	#region Private_Methods
	#endregion

	#region Public_Methods
	public void SetInfo (string name1, string tempUrl, int index, string iconUrl) {

		id = index;
		gamenameText.text = name1;
		StartCoroutine (GetAppIcon (iconUrl));
		URL = tempUrl;
	}

	#endregion

	#region Coroutines
	IEnumerator GetAppIcon (string url) {
		WWW www = new WWW (url);

		yield return www;
		Sprite s;
		var texture = new Texture2D (1024, 1024);
		texture = www.texture;
		s = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (.5f, .5f), 100);
		icon.sprite = s;
		appInfoPopup.texture2D = s.texture;
	}

	public void GotoButtonClick () {
		//int tempid = NBAnalysisApiController.instance.appInfo.apps[id].id;
		//string name = NBAnalysisApiController.instance.appInfo.apps[id].name;
		//string title_of_ad = NBAnalysisApiController.instance.appInfo.apps[id].title_of_ad;
		//string messge_of_ad = NBAnalysisApiController.instance.appInfo.apps[id].messge_of_ad;
		//string link = NBAnalysisApiController.instance.appInfo.apps[id].link;
		//string contact_email = NBAnalysisApiController.instance.appInfo.apps[id].contact_email;
		//string share_format = NBAnalysisApiController.instance.appInfo.apps[id].share_format;
		//string contact_forma = NBAnalysisApiController.instance.appInfo.apps[id].contact_format;
		//string developer_site = NBAnalysisApiController.instance.appInfo.apps[id].developer_site;
		//string developer_name = NBAnalysisApiController.instance.appInfo.apps[id].developer_name;
		//string developer_apps = NBAnalysisApiController.instance.appInfo.apps[id].developer_apps;

		//appInfoPopup.SetInfo (tempid, name, title_of_ad, messge_of_ad, link, contact_email,
		//	share_format, contact_forma, developer_site, developer_name, developer_apps);
	}
	#endregion

	#region Custom_CallBacks
	#endregion
}