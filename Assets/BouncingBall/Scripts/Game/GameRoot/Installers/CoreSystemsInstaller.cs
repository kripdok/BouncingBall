using BouncingBall.DataLoader;
using BouncingBall.Game.Data;
using BouncingBall.PrefabLoader;
using BouncingBall.Utilities;
using BouncingBall.Utilities.Reset;
using Zenject;

namespace BouncingBall.Game.GameRoot.Installers
{
    public class CoreSystemsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();
            BindPrefabLoadStrategy();
            BindLevelLoaderMediator();
            BindDataLoader();
            BindGameBootstrap();
            BindGameDataManager();
            BindResetManager();
        }

        private void BindSceneLoader()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }

        private void BindPrefabLoadStrategy()
        {
            Container.BindInterfacesTo<ResourcesPrefabLoadStrategy>().AsSingle();
        }

        private void BindLevelLoaderMediator()
        {
            Container.Bind<LevelLoaderMediator>().AsSingle();
        }

        private void BindDataLoader()
        {
            Container.Bind<IDataLoader>().To<LocalDataLoader>().AsSingle();
        }

        private void BindGameBootstrap()
        {
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
        }

        private void BindGameDataManager()
        {
            Container.Bind<GameDataManager>().AsSingle();
        }

        private void BindResetManager()
        {
            Container.Bind<ResetManager>().AsSingle();
        }
    }
}
