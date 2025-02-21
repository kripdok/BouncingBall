using BouncingBall.Game.UI.GameplayState;
using BouncingBall.InputSystem.Controller;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
using BouncingBall.Utilities.Reset;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class GameplayState : AbstractGameState
    {
        private const string UIPatch = "Prefabs/UI/Containers/GameUI";

        private readonly IInputInteractivityChanger _manageInputState;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly StateUIFactory _stateUIFactory;
        private readonly LevelLoaderMediator _levelLoaderMediator;
        private readonly ResetManager _resetManager;

        private CompositeDisposable _disposables;

        public GameplayState(ResetManager resetManager,IInputInteractivityChanger manageInputState, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, LevelLoaderMediator levelLoaderMediator, StateUIFactory stateUIFactory) : base(GameStateNames.Gameplay)
        {
            _attachStateUI = attachStateUI;
            _manageInputState = manageInputState;
            _loadingWindowController = loadingWindowController;
            _prefabLoadStrategy = prefabLoadStrategy;
            _levelLoaderMediator = levelLoaderMediator;
            _stateUIFactory = stateUIFactory;
            _resetManager = resetManager;
        }

        public override async void Enter()
        {
            _disposables = new();
            Debug.Log("Начал входить в состояние игры");
            CreateGameUI();
            _levelLoaderMediator.OnLevelLoaded.Where(flag => flag == true).Subscribe(_ => HideLoadingWindow()).AddTo(_disposables);
            _resetManager.Reset();
        }

        public override async UniTask Exit()
        {
            _disposables.Dispose();
            _manageInputState.DisableInput();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateGameUI()
        {
            var prefabGameUI = _prefabLoadStrategy.LoadPrefab<GameUI>(UIPatch);
            var gameUI = _stateUIFactory.Create(prefabGameUI);
            gameUI.OnExit.Subscribe(_ => SetMainMenuState()).AddTo(_disposables);
            _attachStateUI.AttachStateUI(gameUI);
        }

        private void SetMainMenuState()
        {
            OnExit.OnNext(GameStateNames.MainMenu);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
            _manageInputState.EnableInput();
        }
    }
}
