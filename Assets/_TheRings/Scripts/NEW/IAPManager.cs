using OnePF;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    public static IAPManager instace;
    #region Public_Variables
    #endregion

    #region Private_Variables
    #endregion

    #region Events
    #endregion

    #region Unity_CallBacks

    void Awake()
    {
        if (instace)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instace = this;
        }
    }
    void OnEnable() { }
    // Use this for initialization
    void Start()
    {
        MyOpenIAB.instance.Init();
    }
    void OnDisable() { }
    #endregion

    #region Private_Methods

    #endregion

    #region Public_Methods

    public void PurchaseNoAds()
    {
    }

    public void PurchasedSuccess(SkuDetails skuDetails)
    {
        switch (skuDetails.Sku)
        {
            case MyOpenIAB.test_purchase:
                break;
        }
    }

    public void RestoreAllPurchases()
    {
        MyOpenIAB.instance.RestorePurchases();
    }
    #endregion

    #region Coroutines
    #endregion

    #region Custom_CallBacks
    #endregion
}