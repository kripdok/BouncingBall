﻿using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.Entities.BallEntity;
using BouncingBall.Game.Gameplay.Entities.EnemyEntity;
using BouncingBall.Game.Gameplay.LevelObject;
using Zenject;
using Zenject.SpaceFighter;

namespace BouncingBall.Game.Gameplay.Root
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactory();
            Container.Bind<Ball>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball").AsCached();
            Container.Bind<BallDirectionSign>().FromComponentInNewPrefabResource("Prefabs/Gameplay/PointHolder").AsCached().NonLazy();
            Container.Bind<LevelLoader>().AsCached().NonLazy();
            Container.Bind<GameplayBootstrap>().AsCached().NonLazy();
            Container.BindMemoryPool<Coin, CoinsPool>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Coins/Coin").UnderTransformGroup("Coins").AsCached();
            Container.Bind<LevelManager>().AsCached();
            Container.Bind<EnemyPool>().AsCached();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
        }
    }
}
