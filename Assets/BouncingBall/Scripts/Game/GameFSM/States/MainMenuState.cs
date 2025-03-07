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
        private const string LevelId = "0";
        private const string UIPrefabPathc = "Prefabs/UI/Containers/MainMenuUI";

        [Inject] private readonly ILoadingWindowController _loadingWindowController;
        [Inject] private readonly IAttachStateUI _attachStateUI;
        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly LevelLoaderMediator _levelLoaderMediator;
        [Inject] private readonly StateUIFactory _stateUIFactory;
        [Inject] private readonly ResetManager _resetManager;
        [Inject] private readonly IInputInteractivityChanger _inputInteractivityChanger;

        private CompositeDisposable _disposables;
        private string _levelName;

        public MainMenuState() : base(GameStateTag.MainMenu) { }

        public override async void Enter()
        {
            _disposables = new();
            CreateMainMenuUI();
            _levelLoaderMediator.IsLevelLoaded.Where(flag => flag == true).Subscribe(_ => HideLoadingWindow()).AddTo(_disposables);
            _levelLoaderMediator.SetLevelName(LevelId);
            _resetManager.Reset();
            _inputInteractivityChanger.EnableSimulatedInput();
        }

        public override async UniTask Exit()
        {
            _inputInteractivityChanger.DisableSimulatedInput();
            _disposables.Dispose();
            await _loadingWindowController.ShowLoadingWindow();
            _levelLoaderMediator.SetLevelName(_levelName);
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefabSync<MainMenuUI>(UIPrefabPathc);
            var mainMenuUI = _stateUIFactory.Create(prefabMainMenuUI);
            mainMenuUI.OnExit.Subscribe(SetGameplayState).AddTo(_disposables);
            _attachStateUI.AttachStateUI(mainMenuUI);
        }

        private void SetGameplayState(string levelName)
        {
            _levelName = levelName;
            OnExit.OnNext(GameStateTag.Gameplay);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
        }
    }
}
