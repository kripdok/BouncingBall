using BouncingBall.InputSystem;
using BouncingBall.InputSystem.Device;
using Zenject;

namespace BouncingBall.Game.GameRoot.Installers
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputManager();
            BindInputDevicePool();
        }

        private void BindInputManager()
        {
            Container.BindInterfacesTo<InputProvider>().AsSingle();
        }

        private void BindInputDevicePool()
        {
            Container.Bind<InputDevicePool>().AsSingle();
        }
    }
}
