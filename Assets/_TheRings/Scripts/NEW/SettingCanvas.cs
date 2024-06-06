using UnityEngine;
using System.Collections;

public class SettingCanvas : MonoBehaviour
{
    #region Public_Variables
    #endregion

    #region Private_Variables
    #endregion

    #region Events
    #endregion

    #region Unity_CallBacks
    void Awake() { }
    void OnEnable() { }
    // Use this for initialization
    void Start() { }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.settingsCanvas.SetActive(false);
            UIManager.Instance.pauseCanvas.SetActive(true);
        }
    }
    void OnDisable() { }
    #endregion

    #region Private_Methods
    #endregion

    #region Public_Methods
    #endregion

    #region Coroutines
    #endregion

    #region Custom_CallBacks
    #endregion
}
