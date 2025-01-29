using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public class MainMenuState : IState
    {

        private readonly GameStateMachine _gameStateMachine;
        private readonly ILoadingWindowController _loadingWindowController;
        private readonly SceneLoader _sceneLoader;

        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ILoadingWindowController loadingWindowController)
        {
            _gameStateMachine = gameStateMachine;
            _loadingWindowController = loadingWindowController;
            _sceneLoader = new SceneLoader();
        }


        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneNames.Gameplay, EnterLoadLevel);
            await _loadingWindowController.HideLoadingWindow();

            Debug.Log("Состояние главноего меню");
        }

        public void Exit()
        {

        }

        private void EnterLoadLevel()
        {
            //TODO - сделать загрузку главного меню в UI и сцены с катающимся шариком
        }
    }
}
