using System.Collections;
using UnityEngine;

public class MoreAppScreen : MonoBehaviour {

	#region Public_Variables
	public Transform scrollViewContent;
	public GameObject appPrefab;

	#endregion

	#region Private_Variables
	#endregion

	#region Events
	#endregion

	#region Unity_CallBacks
	void Awake () {

	}

	void OnEnable () {

	}

	// Use this for initialization
	void Start () {
		CreateInstanceOfApp ();

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
	public void CreateInstanceOfApp () {

		//int count = NBAnalysisApiController.instance.appInfo.apps.Count;

		//for (int i = 0; i < count; i++) {
		//	GameObject appGameObj = Instantiate (appPrefab);
		//	AppInfoList ap = appGameObj.GetComponent<AppInfoList> ();

		//	string name = NBAnalysisApiController.instance.appInfo.apps[i].title_of_ad;
		//	string url = NBAnalysisApiController.instance.appInfo.apps[i].developer_apps;
		//	string iconUrl = NBAnalysisApiController.instance.appInfo.apps[i].icon_url;

		//	ap.SetInfo (name, url, i, iconUrl);
		//	appGameObj.transform.SetParent (scrollViewContent);
		//	appGameObj.transform.localPosition = Vector3.zero;
		//	appGameObj.transform.localRotation = Quaternion.identity;
		//	appGameObj.transform.localScale = Vector3.one;
		//}
	}

	void ClearList () {
		for (int i = 0; i < scrollViewContent.childCount; i++) {
			Destroy (scrollViewContent.GetChild (i).gameObject);
		}
	}

	public void CloseButton () {
		//	ClearList ();
		gameObject.SetActive (false);
	}

	#endregion

	#region Coroutines
	#endregion

	#region Custom_CallBacks
	#endregion
}