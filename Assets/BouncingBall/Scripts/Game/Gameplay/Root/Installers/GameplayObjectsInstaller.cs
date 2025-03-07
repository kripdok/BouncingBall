using BouncingBall.Game.Gameplay.Entities.BallEntity;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root.Installers
{
    public class GameplayObjectsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBall();
            BindBallDirectionSign();
        }

        private void BindBall()
        {
            Container.Bind<Ball>()
                .FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball")
                .AsCached();
        }

        private void BindBallDirectionSign()
        {
            Container.Bind<BallDirectionSign>()
                .FromComponentInNewPrefabResource("Prefabs/Gameplay/PointHolder")
                .AsCached()
                .NonLazy();
        }
    }
}
