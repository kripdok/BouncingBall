using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.Entities.BallEntity;
using BouncingBall.Game.Gameplay.Entities.EnemyEntity;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root.Installers
{
    public class GameplayPoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindCoinsPool();
            BindEnemyPool();
            BindBallCollisionEffectPool();
        }

        private void BindCoinsPool()
        {
            Container.BindMemoryPool<Coin, CoinsPool>()
                .FromComponentInNewPrefabResource("Prefabs/Gameplay/Coins/Coin")
                .UnderTransformGroup("Coins")
                .AsCached();
        }

        private void BindEnemyPool()
        {
            Container.Bind<EnemyPool>().AsCached();
        }

        private void BindBallCollisionEffectPool()
        {
            Container.BindMemoryPool<BallCollisionEffect, BallCollisionEffectPool>()
                .FromComponentInNewPrefabResource("Prefabs/Gameplay/BallSmokeEffect")
                .UnderTransformGroup("BallCollisionEffect")
                .AsCached();
        }
    }
}
