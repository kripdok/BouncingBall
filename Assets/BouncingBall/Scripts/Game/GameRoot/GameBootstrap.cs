using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot
{
    public class GameBootstrap
    {
        public GameBootstrap(GameStateMachine gameStateMachine , GameDataManager gameDataManager)
        {
            InitStartParams(gameStateMachine, gameDataManager);
        }

        private async void InitStartParams(GameStateMachine gameStateMachine, GameDataManager gameDataManager )
        {
            await gameDataManager.LoadGameData();
            Debug.Log(gameDataManager.GameData);
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            gameStateMachine.SetState<BootstrapState>();
        }
    }
}
