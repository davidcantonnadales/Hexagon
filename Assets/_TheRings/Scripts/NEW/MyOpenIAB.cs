using OnePF;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyOpenIAB : MonoBehaviour
{
    public static MyOpenIAB instance;

    #region Public_Variables
    public string googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAge+pVOM9k7gBIUgDumDkZCbmpkbD7iKqOXCdOMqHiz6yRvCEHbesqOTeSHMFCeyhkT3R7Y9NjQFWbmF13DZnVVYAJlZQaEMC1lgX07vPoQCBuRHw714kgudnYbEzWtiyPpxp/3zNRNXP0kUIn+Cm4Qu6NuFvrdn79zO2zjBcQpLjCsU2VERodaDF9L3g30DCIxylu+DqaqvPZd+7Ioi2k6kom+i0Ff8RT0eN2Vl2SA1MmJGT9pZDkyJMct++GYCb0YFsA6F4gh8obCcOIIUbBwZ2RETQMKM7dcZx0T0SJUkJLl1SzeXuZ8Uv+DC1Y2iA0ISx3civvdizT+L8uUQHpQIDAQAB";
    //  public string iosPublicKey = "";

    public static Inventory _inventory = null;

#if UNITY_ANDROID
 public const string no_ads = "removeads_299";
#elif UNITY_IPHONE
    public const string no_ads = "com.digitalwing.curvedboxes.RemoveAds";
#else
    //  public const string test_purchase = " android.test.purchased";
#endif
    public const string test_purchase = " android.test.purchased";
    // public const string no_ads = "android.test.purchased";

    #endregion

    #region Private_Variables
    bool _isInitialized = false;
    string[] managedItems = new string[] { };
    #endregion

    #region Events
    #endregion

    #region Unity_CallBacks
    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        // Listen to all events for illustration purposes
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
        OpenIABEventManager.transactionRestoredEvent += OnTransactionRestored;
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void OnDisable()
    {
        // Remove all event handlers
        //        OpenIABEventManager.restoreSucceededEvent
        OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
        OpenIABEventManager.transactionRestoredEvent -= OnTransactionRestored;
    }

    #endregion

    #region Private_Methods

    void MapSkus()
    {

    }

    public void Init()
    {
        MapSkus();

        var options = new Options();
        options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
        options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
        options.checkInventory = false;
        options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
        options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_iOS.STORE };
        options.availableStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_iOS.STORE };
        options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_GOOGLE, googlePublicKey } };
        //        options.storeKeys = new Dictionary<string, string> { { OpenIAB_iOS.STORE, iosPublicKey } };
        /* options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_YANDEX, yandexPublicKey } };
         options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_SLIDEME, slideMePublicKey } };*/
        options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;

        // Transmit options and start the service
        OpenIAB.init(options);
    }

    void QueryInventory()
    {

    }

    SkuDetails GetProductDetails(string productId)
    {
        SkuDetails prodDetails = null;
        foreach (var sku in _inventory.GetAllAvailableSkus())
        {
            if (productId.Equals(sku.Sku))
            {
                prodDetails = sku;
                break;
            }
        }
        return prodDetails;
    }

    public void PurchaseProduct(string productId)
    {
        //        PurchaseTest();
        OpenIAB.purchaseProduct(productId);
    }

    void ConsumeProduct(Purchase purchase)
    {
        //        Purchase purchase = new Purchase(producId);
        OpenIAB.consumeProduct(purchase);
    }

    void PurchaseProductSuccess(Purchase purchase)
    {
        string sku = purchase.Sku;
        switch (sku)
        {
            default:
                Debug.LogWarning("Unknown SKU: " + sku);
                break;
        }
    }

    void RestorePurchaseProductSuccess(string sku)
    {
        switch (sku)
        {
            default:
                Debug.LogWarning("Unknown SKU: " + sku);
                break;
        }
    }
    #endregion

    #region Public_Methods

    public void PurchaseTest()
    {
        OpenIAB.purchaseProduct("android.test.purchased");
    }

    public void RestorePurchases()
    {
        OpenIAB.restoreTransactions();
    }
    #endregion

    #region Coroutines
    #endregion

    #region Custom_CallBacks
    private void billingSupportedEvent()
    {
        _isInitialized = true;
        Debug.Log("billingSupportedEvent");
        QueryInventory();
    }

    private void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }

    bool IsSkuManagedItem(string sku)
    {
        return managedItems.Contains(sku);
    }

    private void queryInventorySucceededEvent(Inventory inventory)
    {
        Debug.Log("queryInventorySucceededEvent: " + inventory);
        if (inventory != null)
        {
            _inventory = inventory;

            foreach (var sku in inventory.GetAllPurchases())
            {
                if (IsSkuManagedItem(sku.Sku))
                    ConsumeProduct(sku);
            }
            /*List<Purchase> purchases = _inventory.GetAllPurchases();
            if (purchases != null && purchases.Count > 0)
                purchases.ForEach(x => PurchaseProductSuccess(x.Sku));*/
        }
    }

    private void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
    }

    private void purchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("purchaseSucceededEvent: " + purchase);
        //        if (!IsSkuManagedItem(purchase.Sku))
        PurchaseProductSuccess(purchase);
        //        Init();
    }

    private void purchaseFailedEvent(int errorCode, string errorMessage)
    {
        Debug.Log("purchaseFailedEvent: " + errorMessage);
    }

    private void consumePurchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);

        string sku = purchase.Sku;
        switch (sku)
        {
            default:
                Debug.LogWarning("Unknown SKU: " + sku);
                break;
        }
    }

    private void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
    }

    private void OnTransactionRestored(string sku)
    {
        RestorePurchaseProductSuccess(sku);
    }
    #endregion
}
