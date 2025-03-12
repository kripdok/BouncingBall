using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using System;
using UnityEngine;

namespace BouncingBall.Analytic
{
    public class FirebaseInitializer
    {
        public async UniTask InitializeFirebaseAsync()
        {
            try
            {
                var dependencyStatus = await FirebaseApp.CheckDependenciesAsync();

                if (dependencyStatus != DependencyStatus.Available)
                {
                    throw new Exception("Could not resolve all Firebase dependencies");

                }

                Debug.Log("Firebase initialized successfully");
                ReportLevelCompletion(1.ToString());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void ReportLevelCompletion(string levelName)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter("LevelName", levelName));
            Debug.Log("Report");
        }
    }
}
