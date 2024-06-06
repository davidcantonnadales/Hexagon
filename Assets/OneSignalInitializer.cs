using UnityEngine;

public class OneSignalInitializer : MonoBehaviour
{

    void Start()
    {
        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);

        OneSignal.StartInit("4c132243-61fd-42e3-8624-acf33e2d69d7")
          .HandleNotificationOpened(HandleNotificationOpened)
          .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
}
