using BouncingBall.DataLoader;
using BouncingBall.Game.Data;
using BouncingBall.Game.FinalStateMachine;
using BouncingBall.InputSystem;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
using BouncingBall.Utilities.Reset;
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
            BindCamera();
            Container.Bind<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<ResourcesPrefabLoadStrategy>().AsSingle();
            Container.Bind<LevelLoaderMediator>().AsSingle();
            Container.Bind<IDataLoader>().To<LocalDataLoader>().AsSingle();
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
            Container.Bind<GameDataManager>().AsSingle();
            Container.Bind<ResetManager>().AsSingle();
        }

        private void BindLoadingWindow()
        {
            Container.BindInterfacesTo<UIRootHolder>().FromComponentInNewPrefabResource("Prefabs/UI/UIRoot").AsSingle().NonLazy();
        }

        private void BindCamera()
        {
            Container.Bind<CameraHolder>().FromComponentInNewPrefabResource("Prefabs/CameraHolder").AsSingle().NonLazy();
        }

        private void BindInputController()
        {
            Container.BindInterfacesTo<InputManager>().AsSingle();
            Container.Bind<InputDevicePool>().AsSingle();
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