using System;
using UniRx;
using UnityEngine;
using UnityEngine.Advertisements;

namespace BouncingBall.Ads
{
    public class InterstitialAdsBanner : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public Subject<Unit> AdvertisingHasStarted = new();
        public Subject<Unit> AdvertisingHasEnded = new();

        private string _adUnitId;

        public InterstitialAdsBanner(string adUnitId)
        {
            _adUnitId = adUnitId;
        }

        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd()
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId) { }

        public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string _adUnitId) 
        {
            AdvertisingHasStarted.OnNext(Unit.Default);
        }
        public void OnUnityAdsShowClick(string _adUnitId) { }
        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            AdvertisingHasEnded.OnNext(Unit.Default);
        }
    }
}
