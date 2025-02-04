using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
            Container.Bind<LevelLoader>().AsCached();
            Container.Bind<Ball>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball").AsCached();
            Container.Bind<GameplayBootstrap>().AsCached().NonLazy();
        }
    }
}
