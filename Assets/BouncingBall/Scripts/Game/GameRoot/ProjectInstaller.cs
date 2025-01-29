using Zenject;
using UnityEngine;

namespace BouncingBall.Scripts.Game
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingWindow _loadingWindowPrefab;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();

            BindLoadingWindow();
        }

        private void BindLoadingWindow()
        {
            var loadingWindow = Instantiate(_loadingWindowPrefab);
            DontDestroyOnLoad(loadingWindow);
            Container.Bind<LoadingWindow>().FromInstance(loadingWindow).AsSingle() ;
        }
    }
}