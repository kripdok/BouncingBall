using Assets.BouncingBall.Scripts.Game.Gameplay;
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
            BindFactory();
            Container.Bind<LevelLoader>().AsCached().NonLazy(); 
            Container.Bind<Ball>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball").AsCached();
            Container.Bind<GameplayBootstrap>().AsCached().NonLazy();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
        }
    }
}
