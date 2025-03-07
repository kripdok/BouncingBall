using BouncingBall.Game.FinalStateMachine.States;
using Zenject;

namespace BouncingBall.Game.FinalStateMachine
{
    public class GameStateFactory : IFactory<string, AbstractGameState>
    {
        private DiContainer _container;

        public GameStateFactory(DiContainer container)
        {
            _container = container;
        }

        public AbstractGameState Create(string stateTag)
        {
            return stateTag switch
            {
                GameStateTag.Bootstrap => _container.Instantiate<BootstrapState>(),
                GameStateTag.MainMenu => _container.Instantiate<MainMenuState>(),
                GameStateTag.Gameplay => _container.Instantiate<GameplayState>(),
                _ => null
            };
        }
    }
}
