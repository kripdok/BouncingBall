using BouncingBall.Scripts.Game.Gameplay.Game.UI;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly InputSystemManager _manageInputState;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;


        public GameplayState(GameStateMachine gameStateMachine, InputSystemManager manageInputState, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI)
        {
            _attachStateUI = attachStateUI;
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
        }


        public async void Enter()
        {
            CreateGameUI();
            await _loadingWindowController.HideLoadingWindow();

            _manageInputState.EnableInputSystam();
            Debug.Log("Зашел в геймлпей");

        }

        public async UniTask Exit()
        {
            _manageInputState.DisableInputSystam();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateGameUI()
        {
            var prefabMainMenuUI = Resources.Load<GameUI>("Prefabs/UI/Containers/GameUI");
            var mainMenuUI = GameObject.Instantiate(prefabMainMenuUI);
            mainMenuUI.Init(delegate { _gameStateMachine.SetState<MainMenuState>(); });
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }
    }
}
