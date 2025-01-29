using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot
{
    public class GameBootstrap
    {
        public GameBootstrap(GameStateMachine gameStateMachine)
        {
            InitStartParams();

            gameStateMachine.SetState<BootstrapState>();
        }

        private void InitStartParams()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;
        }
    }
}
