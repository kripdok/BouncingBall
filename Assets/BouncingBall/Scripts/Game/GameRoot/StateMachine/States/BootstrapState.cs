using BouncingBall.Scripts.Game.GameRoot.Constants;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public async void Enter()
        {
            Debug.Log("BootstrapState");
            await _sceneLoader.LoadScene(SceneNames.PreLoader, EnterLoadLevel);
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.SetState<MainMenuState>();
        }
    }
}
