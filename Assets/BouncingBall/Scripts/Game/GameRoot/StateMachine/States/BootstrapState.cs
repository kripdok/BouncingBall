using BouncingBall.Scripts.Game.GameRoot.Constants;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(IStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public string Id => GameStateNames.Bootstrap;

        public async void Enter()
        {
            Debug.Log("BootstrapState");
            await _sceneLoader.LoadScene(SceneNames.PreLoader);
            await _sceneLoader.LoadScene(SceneNames.Gameplay);
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
