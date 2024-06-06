using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfo : MonoBehaviour {

	public static PopupInfo Instance;
	#region Public_Variables
	public Text titleText;
	public Text infoText;
	public string URL;
	public GameObject popupScreen;

	public Button goToStoreButton;
	public Button notNowButton;
	#endregion

	#region Private_Variables
	#endregion

	#region Events
	#endregion

	#region Unity_CallBacks
	void Awake () {
		Instance = this;
	}

	void OnEnable () {
		popupScreen.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		notNowButton.onClick.AddListener (NotNowButtonClick);
		goToStoreButton.onClick.AddListener (GoToStoreButtonClick);
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
	public void SetInfoPopup (string title, string info) {
		titleText.text = title;
		infoText.text = info;
	}
	public void PopupEnableDisable (bool value) {
		popupScreen.SetActive (value);
	}

	public void NotNowButtonClick () {
		popupScreen.SetActive (false);
	}

	public void GoToStoreButtonClick () {
		Application.OpenURL (URL);
	}
	#endregion

	#region Coroutines
	#endregion

	#region Custom_CallBacks
	#endregion
}