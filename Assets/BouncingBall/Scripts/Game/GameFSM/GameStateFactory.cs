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

        public AbstractGameState Create(string param)
        {
            switch (param)
            {
                case GameStateTag.Bootstrap:
                    return _container.Instantiate<BootstrapState>();
                case GameStateTag.MainMenu:
                    return _container.Instantiate<MainMenuState>();
                case GameStateTag.Gameplay:
                    return _container.Instantiate<GameplayState>();
                default:
                    return null;

            }
        }
    }
}
