using BouncingBall.DataLoader;
using BouncingBall.Game.FinalStateMachine;
using BouncingBall.InputSystem;
using BouncingBall.InputSystem.Controller;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
using System;
using UnityEngine;
using Zenject;


namespace BouncingBall.Game.GameRoot.Installers
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
            Container.BindInterfacesTo<GameStateMachine>().AsCached();
            Container.Bind<LevelLoaderMediator>().AsSingle();
            Container.Bind<IDataLoader>().To<LocalDataLoader>().AsSingle();
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
            //TODO - ������� ���� Input
            Container.Bind<InputSystemActions>().AsSingle();
            Container.BindInterfacesTo<InputController>().AsSingle();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, Action, StateUI, StateUIFactory>().FromFactory<PrefabFactory<Action, StateUI>>();
            // Container.BindFactory<Transform, LevelViewModel, LevelView, LevelViewFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/MenuButton").AsCached();
        }
    }
}