using Zenject;


namespace BouncingBall.Game.GameRoot.Installers
{
    public class CameraInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindCameraHolder();
        }

        private void BindCameraHolder()
        {
            Container.Bind<CameraHolder>()
                .FromComponentInNewPrefabResource("Prefabs/CameraHolder")
                .AsSingle()
                .NonLazy();
        }
    }
}
