using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using log4net.Core;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly InputSystemManager _manageInputState;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;


        public GameplayState(GameStateMachine gameStateMachine, InputSystemManager manageInputState, ILoadingWindowController loadingWindowController)
        {
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
        }


        public async void Enter()
        {
            await _loadingWindowController.HideLoadingWindow();
            
            _manageInputState.EnableInputSystam();
            Debug.Log("Зашел в геймлпей");

        }

        public async void Exit()
        {
            _manageInputState.DisableInputSystam();
        }
    }
}
