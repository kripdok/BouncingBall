using BouncingBall.Game.Gameplay.LevelObject;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root.Installers
{
    public class GameplayFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLevelFactory();
        }

        private void BindLevelFactory()
        {
            Container.BindFactory<UnityEngine.Object, Level, LevelFactory>()
                .FromFactory<PrefabFactory<Level>>();
        }
    }
}
