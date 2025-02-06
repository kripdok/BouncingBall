using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.MainMenu.UI;
using BouncingBall.Scripts.Game.Gameplay.Root;
using BouncingBall.Scripts.Game.GameRoot.Constants;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class MainMenuState : IState
    {
        private const string LevelId = "0";
        private const string UIPrefabPathc = "Prefabs/UI/Containers/MainMenuUI";

        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly SceneLoader _sceneLoader;
        private readonly LevelLoaderMediator _levelLoaderMediator;
        private readonly StateUIFactory _stateUIFactory;

        private IDisposable dispos;


        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, StateUIFactory stateUIFactory, LevelLoaderMediator levelLoaderMediator)
        {
            _stateUIFactory = stateUIFactory;
            _levelLoaderMediator = levelLoaderMediator;
            _prefabLoadStrategy = prefabLoadStrategy;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _attachStateUI = attachStateUI;
            _sceneLoader = new SceneLoader();
        }

        public async void Enter()
        {
            Debug.Log("Начал входить в состояние главного меню");
            await _sceneLoader.LoadScene(SceneNames.Gameplay);
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
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }

        private void SetGameplayState()
        {
            _gameStateMachine.SetState<GameplayState>();
        }

        private void HideLoadingWindow()
        {
           
            _loadingWindowController.HideLoadingWindow();
            Debug.Log("Состояние главноего меню");
        }
    }
}
