using BouncingBall.Ads;
using BouncingBall.Analytic;
using BouncingBall.FinalStateMachine;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.GameRoot
{
    public class GameBootstrap
    {
        private const int TargetFrameRate = 60;

        [Inject] private GameDataProvider _gameDataManager;
        [Inject] private IStateMachine _gameStateMachine;
        [Inject] private CameraHolder _cameraHolder;
        [Inject] private AdsMediator _adsMediator;
        [Inject] private FirebaseInitializer _firebaseInitializer;

        [Inject]
        private async UniTaskVoid InitializeGameSettings(GameDataProvider gameDataManager, IStateMachine gameStateMachine, CameraHolder cameraHolder, AdsMediator adsMediator)
        {
            Application.targetFrameRate = TargetFrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            await gameDataManager.LoadGameDataAsync();
            await _firebaseInitializer.InitializeFirebaseAsync();
            cameraHolder.Init();
            adsMediator.Init();
            gameStateMachine.ChangeState(GameStateTag.Bootstrap);
        }
    }
}
