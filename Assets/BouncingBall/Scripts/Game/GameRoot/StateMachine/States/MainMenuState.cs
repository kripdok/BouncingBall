using BouncingBall.Scripts.Game.Gameplay.MainMenu.UI;
using BouncingBall.Scripts.Game.GameRoot.Constants;
using BouncingBall.Scripts.Game.GameRoot.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class MainMenuState : IState
    {

        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly IAttachStateUI _attachStateUI;
        private readonly SceneLoader _sceneLoader;



        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ILoadingWindowController loadingWindowController, IAttachStateUI attachStateUI)
        {
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _attachStateUI = attachStateUI;
            _sceneLoader = new SceneLoader();
        }


        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneNames.Gameplay);
            CreateMainMenuUI();
            await _loadingWindowController.HideLoadingWindow();
            Debug.Log("Состояние главноего меню");

        }

        public async UniTask Exit()
        {
            await _loadingWindowController.ShowLoadingWindow();
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = Resources.Load<MainMenuUI>("Prefabs/UI/Containers/MainMenuUI");
            var mainMenuUI = GameObject.Instantiate(prefabMainMenuUI);
            mainMenuUI.Init(delegate { _gameStateMachine.SetState<GameplayState>(); });
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }
    }
}
