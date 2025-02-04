using BouncingBall.Scripts.Game.Gameplay.Game.UI;
using BouncingBall.Scripts.Game.Gameplay.Root;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class GameplayState : IState
    {
        private const string UIPatch = "Prefabs/UI/Containers/GameUI";

        private readonly IInputInteractivityChanger _manageInputState;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly LevelLoader _levelLoader;


        public GameplayState(GameStateMachine gameStateMachine, IInputInteractivityChanger manageInputState, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, LevelLoader levelLoader)
        {
            _attachStateUI = attachStateUI;
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _levelLoader = levelLoader;
            _prefabLoadStrategy = prefabLoadStrategy;
        }

        public async void Enter()
        {
            CreateGameUI();
            _levelLoader.LoadLevel("1");
            await _loadingWindowController.HideLoadingWindow();

            _manageInputState.EnableInput();
            Debug.Log("Зашел в геймлпей");

        }

        public async UniTask Exit()
        {
            _manageInputState.DisableInput();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateGameUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefab<GameUI>(UIPatch);
            var mainMenuUI = GameObject.Instantiate(prefabMainMenuUI);
            mainMenuUI.Init(delegate { _gameStateMachine.SetState<MainMenuState>(); });
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }
    }
}
