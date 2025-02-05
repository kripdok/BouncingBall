using BouncingBall.Scripts.Game.GameRoot.Constants;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot
{
    public class GameBootstrap
    {
        public  GameBootstrap(SceneLoader sceneLoader)
        {
            InitStartParams(sceneLoader);
        }

        private async void InitStartParams(SceneLoader sceneLoader)
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            await sceneLoader.LoadScene(SceneNames.PreLoader);
            await sceneLoader.LoadScene(SceneNames.Gameplay);
        }

    }
}
