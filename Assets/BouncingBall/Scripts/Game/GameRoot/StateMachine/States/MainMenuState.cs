using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game
{
    public class MainMenuState : IState
    {

        private readonly GameStateMachine _gameStateMachine;
        private readonly LoadingWindow _loadingWindow;
        private readonly SceneLoader _sceneLoader;

        public MainMenuState(GameStateMachine gameStateMachine,SceneLoader sceneLoader,LoadingWindow loadingWindow)
        {
            _gameStateMachine = gameStateMachine;
            _loadingWindow = loadingWindow;
            _sceneLoader = new SceneLoader();
        }


        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneNames.Gameplay, EnterLoadLevel);
            await _loadingWindow.Hide();

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
