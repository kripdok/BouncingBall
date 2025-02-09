using BouncingBall.FinalStateMachine;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine.States;

namespace BouncingBall.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        public GameplayBootstrap(GameDataManager gameDataManager, IStateMachine gameStateMachine)
        {
            LoadData(gameDataManager, gameStateMachine);
        }

        private async void LoadData(GameDataManager gameDataManager, IStateMachine gameStateMachine)
        {
            await gameDataManager.LoadGameData();
            gameStateMachine.SetState(GameStateNames.MainMenu);
        }
    }
}
