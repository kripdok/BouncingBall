using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.MainMenu.UI;
using BouncingBall.Scripts.Game.Gameplay.Root;
using BouncingBall.Scripts.Game.GameRoot.Constants;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using Cysharp.Threading.Tasks;
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
        private readonly LevelLoader _levelLoader;
        private readonly StateUIFactory _stateUIFactory;

        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy, LevelLoader levelLoader, StateUIFactory stateUIFactory)
        {
            _stateUIFactory = stateUIFactory;
            _levelLoader = levelLoader;
            _prefabLoadStrategy = prefabLoadStrategy;
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _attachStateUI = attachStateUI;
            _sceneLoader = new SceneLoader();
        }

        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneNames.Gameplay);
            CreateMainMenuUI();
            await _levelLoader.LoadLevel(LevelId); // Здесь должен быть лоодер, который передает модель уровня для загрузки
            await _loadingWindowController.HideLoadingWindow();
            Debug.Log("Состояние главноего меню");
        }

        public async UniTask Exit()
        {
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefab<MainMenuUI>(UIPrefabPathc);
            var mainMenuUI = _stateUIFactory.Create(prefabMainMenuUI);
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }

    }
}
