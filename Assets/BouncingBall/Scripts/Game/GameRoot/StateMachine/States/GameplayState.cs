using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly InputSystemManager _manageInputState;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;

        private Level _level;


        public GameplayState(GameStateMachine gameStateMachine, InputSystemManager manageInputState, ILoadingWindowController loadingWindowController)
        {
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
        }


        public async void Enter()
        {
            var levelPrefab = Resources.Load<Level>("Prefabs/Gameplay/Levels/Level_1");
            _level = GameObject.Instantiate(levelPrefab);
            await _loadingWindowController.HideLoadingWindow();
            
            _manageInputState.EnableInputSystam();
            Debug.Log("Зашел в геймлпей");

        }

        public async void Exit()
        {
            _manageInputState.DisableInputSystam();
            GameObject.Destroy(_level);
            _level = null;
        }
    }
}
