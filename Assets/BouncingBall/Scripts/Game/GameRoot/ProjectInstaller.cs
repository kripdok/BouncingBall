using BouncingBall.Scripts.Game.GameRoot.StateMachine;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.GameRoot
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();

            BindLoadingWindow();
        }

        private void BindLoadingWindow()
        {
            var uiRootViewPrefab = Resources.Load<UIRootHolder>("Prefabs/UI/UIRoot");
            var uiRootView = Instantiate(uiRootViewPrefab);
            DontDestroyOnLoad(uiRootView);

            Container.Bind<ILoadingWindowController>().FromInstance(uiRootView).AsSingle();
        }
    }
}