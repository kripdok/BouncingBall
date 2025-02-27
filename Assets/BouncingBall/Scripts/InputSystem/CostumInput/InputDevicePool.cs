using BouncingBall.InputSystem.Device;
using System.Collections.Generic;
using Zenject;

namespace BouncingBall.InputSystem
{
    public class InputDevicePool : IFactory<InputDeviceTag, IInputDevice>
    {
        private DiContainer _container;
        private Dictionary<InputDeviceTag, IInputDevice> _devices = new();

        public InputDevicePool(DiContainer container)
        {
            _container = container;
        }

        public IInputDevice Create(InputDeviceTag param)
        {
            if (_devices.TryGetValue(param, out var inputDevice))
            {
                return inputDevice;
            }

            switch (param)
            {
                case InputDeviceTag.Keyboard:
                    return CreateNewInputDevice<KeyboardInputDevice>(param);
                case InputDeviceTag.Mouse:
                    return CreateNewInputDevice<MouseInputDevice>(param);
                case InputDeviceTag.Touchpad:
                    return CreateNewInputDevice<TouchpadInputDevice>(param);
                case InputDeviceTag.Simulator:
                    return CreateNewInputDevice<PlayerInputSimulator>(param);
                default:
                    return null;

            }
        }

        private IInputDevice CreateNewInputDevice<T>(InputDeviceTag name) where T : IInputDevice
        {
            var inputDevuce = _container.Instantiate<T>();
            _devices.Add(name, inputDevuce);
            return inputDevuce;
        }
    }
}
