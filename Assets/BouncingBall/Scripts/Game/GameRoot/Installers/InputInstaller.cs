using BouncingBall.InputSystem;
using BouncingBall.InputSystem.Device;
using Zenject;

namespace Assets.BouncingBall.Scripts.Game.GameRoot.Installers
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
            Container.BindInterfacesTo<InputManager>().AsSingle();
        }

        private void BindInputDevicePool()
        {
            Container.Bind<InputDevicePool>().AsSingle();
        }
    }
}
