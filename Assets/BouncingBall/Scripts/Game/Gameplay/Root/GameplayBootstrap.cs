using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        public GameplayBootstrap(GameDataManager gameDataManager , IStateMachine gameStateMachine)
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
