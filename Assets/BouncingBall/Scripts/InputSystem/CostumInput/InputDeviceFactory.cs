using Zenject;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public class InputDeviceFactory : IFactory<InputDeviceName, IInputDevice>
    {
        private DiContainer _container;

        public InputDeviceFactory(DiContainer container)
        {
            _container = container;
        }

        public IInputDevice Create(InputDeviceName param)
        {
            switch(param)
            {
                case InputDeviceName.Keyboard:
                    return _container.Instantiate<KeyboardInputDevice>();
                case InputDeviceName.Mouse:
                    return _container.Instantiate<MouseInputDevice>();
                case InputDeviceName.Touchpad:
                    return _container.Instantiate<TouchpadInputDevice>();
                case InputDeviceName.Simulator:
                    return _container.Instantiate<PlayerInputSimulator>();
                default:
                    return null;

            }
        }
    }
}
