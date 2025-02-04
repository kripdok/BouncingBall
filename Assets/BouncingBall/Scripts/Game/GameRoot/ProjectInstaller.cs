using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem;
using BouncingBall.Scripts.InputSystem.Controller;
using UnityEngine;
using Zenject;
using BouncingBall.Scripts.Game.Gameplay.Root;

namespace BouncingBall.Scripts.Game.GameRoot
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLoadingWindow();
            BindInputController();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<ResourcesPrefabLoadStrategy>().AsSingle();
            Container.BindFactory<Object, Level, LevelFactory>().FromFactory<PrefabFactory<Level>>();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<LevelLoader>().AsSingle();
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
            Container.Bind<InputSystemActions>().AsSingle();
            Container.Bind<InputSystemManager>().AsSingle();
            Container.Bind<InputController>().AsSingle();
        }
    }
}