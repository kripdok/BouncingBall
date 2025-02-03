using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem;
using BouncingBall.Scripts.InputSystem.Controller;
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
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
        }

        private void BindLoadingWindow()
        {
            var uiRootViewPrefab = Resources.Load<UIRootHolder>("Prefabs/UI/UIRoot");
            var uiRootView = Instantiate(uiRootViewPrefab);
            DontDestroyOnLoad(uiRootView);

            Container.Bind<ILoadingWindowController>().FromInstance(uiRootView).AsSingle();
        }

        private void BindInputController()
        {
            Container.Bind<InputSystemActions>().AsSingle();
            Container.Bind<InputSystemManager>().AsSingle();
            Container.Bind<InputController>().AsSingle();
        }
    }
}