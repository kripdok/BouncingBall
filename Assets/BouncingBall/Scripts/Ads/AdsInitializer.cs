using UnityEngine;
using UnityEngine.Advertisements;

namespace BouncingBall.Ads
{
    public class AdsInitializer : IUnityAdsInitializationListener
    {
        private bool _testMode;
        private string _gameId;

        public AdsInitializer(bool testMode, string gameId)
        {
            _testMode = testMode;
            _gameId = gameId;
        }

        public void Init()
        {

            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Debug.Log("Unity Ads started to initialize");
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}
