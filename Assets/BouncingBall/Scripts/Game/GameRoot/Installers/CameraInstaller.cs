using BouncingBall.Game.GameRoot;
using Zenject;

namespace Assets.BouncingBall.Scripts.Game.GameRoot.Installers
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
