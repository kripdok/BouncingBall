using BouncingBall.Game.UI.MainMenuState;
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
    public class MainMenuState : AbstractGameState
    {
        private const string LevelId = "0";
        private const string UIPrefabPathc = "Prefabs/UI/Containers/MainMenuUI";

        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly LevelLoaderMediator _levelLoaderMediator;
        private readonly StateUIFactory _stateUIFactory;
        private readonly ResetManager _resetManager;

        private CompositeDisposable _disposables;

        public MainMenuState(ResetManager resetManager,ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, StateUIFactory stateUIFactory, LevelLoaderMediator levelLoaderMediator) : base(GameStateNames.MainMenu)
        {
            _stateUIFactory = stateUIFactory;
            _levelLoaderMediator = levelLoaderMediator;
            _prefabLoadStrategy = prefabLoadStrategy;
            _loadingWindowController = loadingWindowController;
            _attachStateUI = attachStateUI;
            _resetManager = resetManager;
        }

        public override async void Enter()
        {
            _disposables = new();
            Debug.Log("Начал входить в состояние главного меню");
            CreateMainMenuUI();
            _levelLoaderMediator.OnLevelLoaded.Where(flag => flag == true).Subscribe(_ => HideLoadingWindow()).AddTo(_disposables);
            _levelLoaderMediator.SetLevelName(LevelId);
            _resetManager.Reset();
        }

        public override async UniTask Exit()
        {
            _disposables.Dispose();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefab<MainMenuUI>(UIPrefabPathc);
            var mainMenuUI = _stateUIFactory.Create(prefabMainMenuUI);
            mainMenuUI.OnExit.Subscribe(_ => SetGameplayState()).AddTo(_disposables);
            _attachStateUI.AttachStateUI(mainMenuUI);
        }

        private void SetGameplayState()
        {
            OnExit.OnNext(GameStateNames.Gameplay);
        }

        private void HideLoadingWindow()
        {
            _loadingWindowController.HideLoadingWindow();
            Debug.Log("Состояние главноего меню");
        }
    }
}
