using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.Game.UI;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using System;

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
        private readonly StateUIFactory _stateUIFactory;
        private readonly LevelLoaderMediator _levelLoaderMediator;

        private IDisposable dispos;

        public GameplayState(GameStateMachine gameStateMachine, IInputInteractivityChanger manageInputState, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, LevelLoaderMediator levelLoaderMediator, StateUIFactory stateUIFactory)
        {
            _attachStateUI = attachStateUI;
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _prefabLoadStrategy = prefabLoadStrategy;
            _levelLoaderMediator = levelLoaderMediator;
            _stateUIFactory = stateUIFactory;
        }

        public async void Enter()
        {
            Debug.Log("Начал входить в состояние игры");
            CreateGameUI();
            dispos = _levelLoaderMediator.OnLevelLoaded.Where(flag => flag == true).Subscribe(_ => HideLoadingWindow());
        }

        public async UniTask Exit()
        {
            dispos.Dispose();
            _manageInputState.DisableInput();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateGameUI()
        {
            var prefabGameUI = _prefabLoadStrategy.LoadPrefab<GameUI>(UIPatch);
            var gameUI = _stateUIFactory.Create(prefabGameUI, delegate { SetMainMenuState(); });
            _attachStateUI.AttachStateUI(gameUI.gameObject);
        }

        private void SetMainMenuState()
        {
            _gameStateMachine.SetState<MainMenuState>();
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
            _manageInputState.EnableInput();
            Debug.Log("Зашел в геймлпей");
        }
    }
}
