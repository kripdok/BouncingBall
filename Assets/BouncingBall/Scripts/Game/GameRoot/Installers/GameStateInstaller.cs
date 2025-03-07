using BouncingBall.Game.FinalStateMachine;
using Zenject;

namespace Assets.BouncingBall.Scripts.Game.GameRoot.Installers
{
    public class GameStateInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameStateFactory();
            BindGameStateMachine();
        }

        private void BindGameStateFactory()
        {
            Container.Bind<GameStateFactory>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }
    }
}
