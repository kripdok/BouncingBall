using Assets.BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<UnityEngine.Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
            Container.Bind<LevelLoader>().AsCached();
            Container.Bind<GameStateMachine>().AsCached();
            Container.Bind<Ball>().FromComponentInNewPrefabResource("Prefabs/Gameplay/Ball").AsCached();
            Container.Bind<GameplayBootstrap>().AsCached().NonLazy();
            Container.BindFactory<Transform,LevelViewModel, LevelView, LevelViewFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/MenuButton").AsCached();
            Container.BindFactory<UnityEngine.Object,Action, StateUI, StateUIFactory>().FromFactory<PrefabFactory<Action, StateUI >> ();
        }
    }
}
