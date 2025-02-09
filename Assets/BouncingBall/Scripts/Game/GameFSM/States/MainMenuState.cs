using BouncingBall.FinalStateMachine;
using BouncingBall.Game.UI.MainMenuState;
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
    public class MainMenuState : IState
    {
        private const string LevelId = "0";
        private const string UIPrefabPathc = "Prefabs/UI/Containers/MainMenuUI";

        private readonly IStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly LevelLoaderMediator _levelLoaderMediator;
        private readonly StateUIFactory _stateUIFactory;

        private IDisposable dispos;


        public MainMenuState(IStateMachine gameStateMachine, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, StateUIFactory stateUIFactory, LevelLoaderMediator levelLoaderMediator)
        {
            _stateUIFactory = stateUIFactory;
            _levelLoaderMediator = levelLoaderMediator;
            _prefabLoadStrategy = prefabLoadStrategy;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _attachStateUI = attachStateUI;
        }

        public string Id => GameStateNames.MainMenu;

        public async void Enter()
        {
            Debug.Log("Начал входить в состояние главного меню");
            CreateMainMenuUI();
            _levelLoaderMediator.SetLevelName(LevelId);
            dispos = _levelLoaderMediator.OnLevelLoaded.Where(flag => flag == true).Subscribe(_ => HideLoadingWindow());
        }

        public async UniTask Exit()
        {
            dispos.Dispose();
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefab<MainMenuUI>(UIPrefabPathc);
            var mainMenuUI = _stateUIFactory.Create(prefabMainMenuUI, delegate { SetGameplayState(); });
            _attachStateUI.AttachStateUI(mainMenuUI);
        }

        private void SetGameplayState()
        {
            _gameStateMachine.SetState(GameStateNames.Gameplay);
        }

        private void HideLoadingWindow()
        {

            _loadingWindowController.HideLoadingWindow();
            Debug.Log("Состояние главноего меню");
        }
    }
}
