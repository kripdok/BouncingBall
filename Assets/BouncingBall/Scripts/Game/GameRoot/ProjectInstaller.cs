using Assets.BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using System;
using UnityEngine;
using Zenject;


namespace BouncingBall.Scripts.Game.GameRoot
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLoadingWindow();
            BindInputController();
            BindFactory();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<ResourcesPrefabLoadStrategy>().AsSingle();
            Container.Bind<GameInformation>().AsSingle();
            Container.Bind<GameStateMachine>().AsCached();
            Container.Bind<LevelLoaderMediator>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
        }

        private void BindLoadingWindow()
        {
            var uiRootViewPrefab = Resources.Load<UIRootHolder>("Prefabs/UI/UIRoot");
            var uiRootView = Instantiate(uiRootViewPrefab);
            DontDestroyOnLoad(uiRootView);

            Container.BindInterfacesTo<UIRootHolder>().FromInstance(uiRootView).AsSingle();
        }

        private void BindInputController()
        {
            //TODO - Сделать свой Input
            Container.Bind<InputSystemActions>().AsSingle();
            Container.BindInterfacesTo<InputController>().AsSingle();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, Action, StateUI, StateUIFactory>().FromFactory<PrefabFactory<Action, StateUI>>();
            Container.BindFactory<Transform, LevelViewModel, LevelView, LevelViewFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/MenuButton").AsCached();
        }
    }
}