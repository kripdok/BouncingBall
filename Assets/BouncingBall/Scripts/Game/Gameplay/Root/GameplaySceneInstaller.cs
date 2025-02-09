using BouncingBall.Game.Data;
using BouncingBall.Game.Gameplay.BallObject;
using BouncingBall.Game.Gameplay.LevelObject;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactory();
            Container.Bind<Ball>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball").AsCached();
            Container.Bind<LevelLoader>().AsCached().NonLazy();
            Container.Bind<GameplayBootstrap>().AsCached().NonLazy();
            Container.Bind<GameDataManager>().AsCached();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
        }
    }
}
