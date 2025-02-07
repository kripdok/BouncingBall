using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        public GameplayBootstrap(GameDataManager gameDataManager , GameStateMachine gameStateMachine)
        {
            LoadData(gameDataManager, gameStateMachine);
        }

        private async void LoadData(GameDataManager gameDataManager, GameStateMachine gameStateMachine)
        {
            await gameDataManager.LoadGameData();
            gameStateMachine.SetState<MainMenuState>();
        }
    }
}
