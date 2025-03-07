using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine;

namespace BouncingBall.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        public GameplayBootstrap(IStateMachine gameStateMachine)
        {
            gameStateMachine.ChangeStateAsync(GameStateTag.MainMenu);
        }
    }
}
