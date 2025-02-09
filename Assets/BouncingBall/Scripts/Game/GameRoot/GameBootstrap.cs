using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;
using UnityEngine;

namespace BouncingBall.Game.GameRoot
{
    public class GameBootstrap
    {
        public GameBootstrap(IStateMachine gameStateMachine)
        {
            InitStartParams(gameStateMachine);
        }

        private async void InitStartParams(IStateMachine gameStateMachine)
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            gameStateMachine.SetState(GameStateNames.Bootstrap);
        }
    }
}
