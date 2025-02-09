using BouncingBall.FinalStateMachine;
using BouncingBall.Game.UI.GameplayState;
using BouncingBall.InputSystem.Controller;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class GameplayState : IState
    {
        private const string UIPatch = "Prefabs/UI/Containers/GameUI";

        private readonly IInputInteractivityChanger _manageInputState;
        private readonly IStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly StateUIFactory _stateUIFactory;
        private readonly LevelLoaderMediator _levelLoaderMediator;

        private IDisposable dispos;

        public GameplayState(IStateMachine gameStateMachine, IInputInteractivityChanger manageInputState, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, LevelLoaderMediator levelLoaderMediator, StateUIFactory stateUIFactory)
        {
            _attachStateUI = attachStateUI;
            _manageInputState = manageInputState;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _prefabLoadStrategy = prefabLoadStrategy;
            _levelLoaderMediator = levelLoaderMediator;
            _stateUIFactory = stateUIFactory;
        }

        public string Id => GameStateNames.Gameplay;
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
            _attachStateUI.AttachStateUI(gameUI);
        }

        private void SetMainMenuState()
        {
            _gameStateMachine.SetState(GameStateNames.MainMenu);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
            _manageInputState.EnableInput();
            Debug.Log("Зашел в геймлпей");
        }
    }
}
