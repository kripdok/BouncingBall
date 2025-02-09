using BouncingBall.FinalStateMachine;
using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Game.FinalStateMachine.States
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
