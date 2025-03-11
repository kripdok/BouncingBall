using BouncingBall.Ads;
using BouncingBall.FinalStateMachine;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Game.GameRoot
{
    public class GameBootstrap
    {
        private const int TargetFrameRate = 60;

        public GameBootstrap(GameDataManager gameDataManager, IStateMachine gameStateMachine, CameraHolder cameraHolder, AdsMediator adsMediator)
        {
            InitializeGameSettings(gameDataManager, gameStateMachine, cameraHolder, adsMediator).Forget();
        }

        private async UniTaskVoid InitializeGameSettings(GameDataManager gameDataManager, IStateMachine gameStateMachine, CameraHolder cameraHolder, AdsMediator adsMediator)
        {
            Application.targetFrameRate = TargetFrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            await gameDataManager.LoadGameDataAsync();
            cameraHolder.Init();
            adsMediator.Init();
            gameStateMachine.ChangeState(GameStateTag.Bootstrap);
        }
    }
}
