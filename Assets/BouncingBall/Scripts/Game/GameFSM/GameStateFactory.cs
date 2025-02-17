using BouncingBall.FinalStateMachine;
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
                case GameStateNames.Bootstrap:
                    return _container.Instantiate<BootstrapState>();
                case GameStateNames.MainMenu:
                    return _container.Instantiate<MainMenuState>();
                case GameStateNames.Gameplay:
                    return _container.Instantiate<GameplayState>();
                default:
                    return null;

            }
        }
    }
}
