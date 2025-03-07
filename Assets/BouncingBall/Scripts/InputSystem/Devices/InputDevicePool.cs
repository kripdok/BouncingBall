using BouncingBall.InputSystem.Device.Concrete;
using System.Collections.Generic;
using Zenject;

namespace BouncingBall.InputSystem.Device
{
    public class InputDevicePool : IFactory<InputDeviceTag, IInputDevice>
    {
        private readonly DiContainer _container;
        private readonly Dictionary<InputDeviceTag, IInputDevice> _cachedDevices = new();

        public InputDevicePool(DiContainer container)
        {
            _container = container;
        }

        public IInputDevice Create(InputDeviceTag deviceTag)
        {
            if (_cachedDevices.TryGetValue(deviceTag, out var inputDevice))
            {
                inputDevice.Reset();
                return inputDevice;
            }

            return CreateAndCacheInputDevice(deviceTag);
        }

        private IInputDevice CreateAndCacheInputDevice(InputDeviceTag deviceTag)
        {
            IInputDevice inputDevice = deviceTag switch
            {
                InputDeviceTag.Keyboard => _container.Instantiate<KeyboardInputDevice>(),
                InputDeviceTag.Mouse => _container.Instantiate<MouseInputDevice>(),
                InputDeviceTag.Touchpad => _container.Instantiate<TouchpadInputDevice>(),
                InputDeviceTag.Simulator => _container.Instantiate<PlayerInputSimulator>(),
                _ => null
            };

            if (inputDevice != null)
            {
                _cachedDevices.Add(deviceTag, inputDevice);
            }

            return inputDevice;
        }

    }
}
