using BouncingBall.InputSystem.Controller;
using BouncingBall.InputSystem.Device;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem
{
    public class InputManager : IInputInteractivityChanger, IInputManager
    {
        public ReadOnlyReactiveProperty<Vector3> RotationAmount { get; private set; }
        public ReadOnlyReactiveProperty<float> ZScale { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReadOnlyReactiveProperty<float> Angle { get; private set; }

        private Subject<Unit> _inputChange = new();
        private CompositeDisposable _disposable;
        private InputDevicePool _factory;
        private IInputDevice _testInputDevice;
        private InputDeviceTag _currentInputDeviceName;

        public ISubject<Unit> InputChange => _inputChange;

        public InputManager(InputDevicePool factory)
        {
            _factory = factory;
            InitializeInputDevice(InputDeviceTag.Keyboard);
        }

        public void EnableInput()
        {
            _disposable = new CompositeDisposable();
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposable);

            if (_testInputDevice == null)
            {
                InitializeInputDevice(_currentInputDeviceName);
            }
        }

        public void DisableInput()
        {
            _disposable.Dispose();
            RotationAmount.Dispose();
            ZScale.Dispose();
            IsDirectionSet.Dispose();
            Angle.Dispose();
            _testInputDevice = null;
        }

        public void EnableControllable()
        {
            if (_testInputDevice is IControllable controllableDevice)
            {
                controllableDevice.EnableControllable();
            }
        }

        private void Update()
        {
            CheckInputDevice();

            _testInputDevice?.SetRotationAndScale();
            _testInputDevice?.TryDisableIsDirectionSet();
        }

        private void CheckInputDevice()
        {
            InputDeviceTag newInputDeviceName;

            if (Input.GetMouseButton(0))
            {
                newInputDeviceName = InputDeviceTag.Mouse;
            }
            else if (Input.anyKey)
            {
                newInputDeviceName = InputDeviceTag.Keyboard;
            }
            else if (Input.touchCount > 0)
            {
                newInputDeviceName = InputDeviceTag.Touchpad;
            }
            else
            {
                return;
            }

            if (newInputDeviceName != _currentInputDeviceName)
            {
                InitializeInputDevice(newInputDeviceName);
            }
        }

        private void InitializeInputDevice(InputDeviceTag deviceName)
        {
            _currentInputDeviceName = deviceName;
            _testInputDevice = _factory.Create(deviceName);

            RotationAmount = new ReadOnlyReactiveProperty<Vector3>(_testInputDevice.Direction);
            ZScale = new ReadOnlyReactiveProperty<float>(_testInputDevice.ZScale);
            IsDirectionSet = new ReadOnlyReactiveProperty<bool>(_testInputDevice.IsDirectionSet);
            Angle = new ReadOnlyReactiveProperty<float>(_testInputDevice.Angle);

            _inputChange.OnNext(Unit.Default);
        }
    }
}