using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class BootstrapState : AbstractGameState
    {
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(SceneLoader sceneLoader) : base(GameStateNames.Bootstrap)
        {
            _sceneLoader = sceneLoader;
        }

        public override async void Enter()
        {
            Debug.Log("BootstrapState");
            await _sceneLoader.LoadScene(SceneNames.PreLoader);
            await _sceneLoader.LoadScene(SceneNames.Gameplay);
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
