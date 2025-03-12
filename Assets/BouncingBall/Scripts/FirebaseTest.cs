using Firebase.Analytics;
using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    private Firebase.FirebaseApp app;

    
    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                 app = Firebase.FirebaseApp.DefaultInstance;
                Test();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void Test()
    {
        Debug.Log("TEst");
        FirebaseAnalytics.LogEvent("Test");
        
    }
}
