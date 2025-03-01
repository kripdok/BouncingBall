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
using Zenject;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class GameplayState : AbstractGameState
    {
        private const string UIPatch = "Prefabs/UI/Containers/GameUI";

        [Inject] private readonly ILoadingWindowController _loadingWindowController;
        [Inject] private readonly IAttachStateUI _attachStateUI;
        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly StateUIFactory _stateUIFactory;
        [Inject] private readonly LevelLoaderMediator _levelLoaderMediator;
        [Inject] private readonly ResetManager _resetManager;
        [Inject] private readonly IInputInteractivityChanger _manageInputState;

        private CompositeDisposable _disposables;

        public GameplayState() : base(GameStateTag.Gameplay) { }

        public override async void Enter()
        {
            _manageInputState.EnableInput();
            _disposables = new();
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
            OnExit.OnNext(GameStateTag.MainMenu);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
        }
    }
}
