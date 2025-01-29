using UnityEngine;

namespace BouncingBall.Scripts.Game
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
