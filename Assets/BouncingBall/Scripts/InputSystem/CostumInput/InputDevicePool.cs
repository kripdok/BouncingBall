using System.Collections.Generic;
using Zenject;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public class InputDevicePool : IFactory<InputDeviceName, IInputDevice>
    {
        private DiContainer _container;
        private Dictionary<InputDeviceName, IInputDevice> _devices = new();

        public InputDevicePool(DiContainer container)
        {
            _container = container;
        }

        public IInputDevice Create(InputDeviceName param)
        {
            if (_devices.TryGetValue(param, out var inputDevice))
            {
                return inputDevice;
            }

            switch (param)
            {
                case InputDeviceName.Keyboard:
                    return CreateNewInputDevice<KeyboardInputDevice>(param);
                case InputDeviceName.Mouse:
                    return CreateNewInputDevice<MouseInputDevice>(param);
                case InputDeviceName.Touchpad:
                    return CreateNewInputDevice<TouchpadInputDevice>(param);
                case InputDeviceName.Simulator:
                    return CreateNewInputDevice<PlayerInputSimulator>(param);
                default:
                    return null;

            }
        }

        private IInputDevice CreateNewInputDevice<T>(InputDeviceName name) where T : IInputDevice
        {
            var inputDevuce = _container.Instantiate<T>();
            _devices.Add(name, inputDevuce);
            return inputDevuce;
        }
    }
}
