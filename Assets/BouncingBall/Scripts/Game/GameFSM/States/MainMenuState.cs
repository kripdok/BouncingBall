using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Game.UI.MainMenuState;
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
    public class MainMenuState : AbstractGameState
    {
        private const string MainMenuUIPrefabPath = "Prefabs/UI/MainMenu/MainMenuUI";

        [Inject] private readonly ILoadingWindowController _loadingWindowController;
        [Inject] private readonly IAttachStateUI _attachStateUI;
        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly LevelLoaderMediator _levelLoaderMediator;
        [Inject] private readonly StateUIFactory _stateUIFactory;
        [Inject] private readonly ResetProvider _resetManager;
        [Inject] private readonly IInputInteractivityChanger _inputInteractivityChanger;

        private CompositeDisposable _subscriptions;
        private string _selectedLevelName;

        public override async void Enter()
        {
            _subscriptions = new CompositeDisposable();

            InitializeMainMenuUI();

            _levelLoaderMediator.IsLevelLoaded
                .Where(isLoaded => isLoaded)
                .Subscribe(_ => HideLoadingWindow())
                .AddTo(_subscriptions);

            _levelLoaderMediator.SetLevelName(MainMenuTag.Name);
            _resetManager.Reset();
            _inputInteractivityChanger.EnableSimulatedInput();
        }

        public override async UniTask Exit()
        {
            _inputInteractivityChanger.DisableSimulatedInput();
            _subscriptions.Dispose();
            await _loadingWindowController.ShowLoadingScreen();
            _levelLoaderMediator.SetLevelName(_selectedLevelName);
        }

        private void InitializeMainMenuUI()
        {
            var mainMenuUIPrefab = _prefabLoadStrategy.LoadPrefabSync<MainMenuUI>(MainMenuUIPrefabPath);
            var mainMenuUI = _stateUIFactory.Create(mainMenuUIPrefab);

            mainMenuUI.ExitRequested
                .Subscribe(HandleTransitionToGameplayState)
                .AddTo(_subscriptions);

            _attachStateUI.AttachStateUI(mainMenuUI);
        }

        private void HandleTransitionToGameplayState(string levelName)
        {
            _selectedLevelName = levelName;
            OnExit.OnNext(GameStateTag.Gameplay);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingScreen();
        }
    }
}
