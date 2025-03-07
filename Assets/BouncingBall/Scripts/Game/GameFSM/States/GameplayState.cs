using BouncingBall.Game.UI.GameplayState;
using BouncingBall.InputSystem;
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
        private const string GameUIPrefabPath = "Prefabs/UI/Containers/GameUI";

        [Inject] private readonly ILoadingWindowController _loadingWindowController;
        [Inject] private readonly IAttachStateUI _attachStateUI;
        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly StateUIFactory _stateUIFactory;
        [Inject] private readonly LevelLoaderMediator _levelLoaderMediator;
        [Inject] private readonly ResetManager _resetManager;
        [Inject] private readonly IInputInteractivityChanger _inputInteractivityChanger;

        private CompositeDisposable _subscriptions;

        public GameplayState() : base(GameStateTag.Gameplay) { }

        public override async void Enter()
        {
            _inputInteractivityChanger.EnableInput();
            _subscriptions = new CompositeDisposable();

            InitializeGameUI();

            _levelLoaderMediator.IsLevelLoaded
                .Where(isLoaded => isLoaded)
                .Subscribe(_ => HideLoadingScreen())
                .AddTo(_subscriptions);

            _resetManager.Reset();
        }

        public override async UniTask Exit()
        {
            _subscriptions.Dispose();
            _inputInteractivityChanger.DisableInput();
            await _loadingWindowController.ShowLoadingScreen();
        }

        private void InitializeGameUI()
        {
            var gameUIPrefab = _prefabLoadStrategy.LoadPrefabSync<GameUI>(GameUIPrefabPath);
            var gameUI = _stateUIFactory.Create(gameUIPrefab);

            gameUI.OnExit
                .Subscribe(_ => TransitionToMainMenuState())
                .AddTo(_subscriptions);

            _attachStateUI.AttachStateUI(gameUI);
        }

        private void TransitionToMainMenuState()
        {
            OnExit.OnNext(GameStateTag.MainMenu);
        }

        private void HideLoadingScreen()
        {
            _loadingWindowController.HideLoadingScreen();
        }
    }
}
