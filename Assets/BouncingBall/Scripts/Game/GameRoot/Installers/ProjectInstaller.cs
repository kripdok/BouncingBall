using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.DataLoader;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
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
            BindGameFSM();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<ResourcesPrefabLoadStrategy>().AsSingle();
            Container.Bind<LevelLoaderMediator>().AsSingle();
            Container.Bind<IDataLoader>().To<LocalDataLoader>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
            Container.Bind<GameDataManager>().AsSingle();
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
            Container.BindInterfacesTo<InputManager>().AsSingle();
            Container.Bind<InputDeviceFactory>().AsSingle();
        }

        private void BindFactory()
        {
            Container.BindFactory<UnityEngine.Object, StateUI, StateUIFactory>().FromFactory<PrefabFactory<StateUI>>();
        }

        private void BindGameFSM()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }
    }
}