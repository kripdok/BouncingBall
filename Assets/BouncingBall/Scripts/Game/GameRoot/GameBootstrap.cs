using BouncingBall.FinalStateMachine;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine.States;
using UnityEngine;

namespace BouncingBall.Game.GameRoot
{
    public class GameBootstrap
    {
        public GameBootstrap(GameDataManager gameDataManager, IStateMachine gameStateMachine, CameraHolder cameraHolder)
        {
            InitStartParams(gameDataManager, gameStateMachine, cameraHolder);
        }

        private async void InitStartParams(GameDataManager gameDataManager, IStateMachine gameStateMachine, CameraHolder cameraHolder)
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            await gameDataManager.LoadGameData();
            cameraHolder.Init();
            gameStateMachine.SetState(GameStateNames.Bootstrap);
        }
    }
}
