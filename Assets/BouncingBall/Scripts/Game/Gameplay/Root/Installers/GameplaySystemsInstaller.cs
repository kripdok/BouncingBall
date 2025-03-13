using Zenject;

namespace BouncingBall.Game.Gameplay.Root.Installers
{
    public class GameplaySystemsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLevelLoader();
            BindLevelManager();
            BindGameplayBootstrap();
        }

        private void BindLevelLoader()
        {
            Container.Bind<LevelLoader>()
                .AsCached()
                .NonLazy();
        }

        private void BindLevelManager()
        {
            Container.Bind<LevelProvider>().AsCached();
        }

        private void BindGameplayBootstrap()
        {
            Container.Bind<GameplayBootstrap>()
                .AsCached()
                .NonLazy();
        }
    }
}
