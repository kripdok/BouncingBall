using Cysharp.Threading.Tasks;
using Firebase;
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
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
