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
        private IInputDevice _inputDevice;
        private InputDeviceTag _currentInputDeviceName;

        public ISubject<Unit> InputChange => _inputChange;

        public InputManager(InputDevicePool factory)
        {
            _factory = factory;
            _currentInputDeviceName = InputDeviceTag.Keyboard;
           // InitializeInputDevice(InputDeviceTag.Keyboard);
        }

        public void EnableInput()
        {
            Debug.Log("Enable input");
            _disposable = new CompositeDisposable();
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposable);

            if (_inputDevice == null)
            {
                InitializeInputDevice(_currentInputDeviceName);
            }
        }

        public void DisableInput()
        {
            Debug.Log("Disable input");
            _disposable?.Dispose();
            RotationAmount?.Dispose();
            ZScale?.Dispose();
            IsDirectionSet?.Dispose();
            Angle?.Dispose();
            _inputDevice = null;
        }

        public void EnableControllable()
        {
            if (_inputDevice is IControllable controllableDevice)
            {
                controllableDevice.EnableControllable();
            }
        }

        public void EnableInputSimulator()
        {
            InitializeInputDevice(InputDeviceTag.Simulator);
            if (_inputDevice is PlayerInputSimulator input)
            {
                input.Simulate();
            }
        }

        public void DisableInputSimulator()
        {
            if(_inputDevice is PlayerInputSimulator input)
            {
                input.Disable();
                DisableInput();
            }
        }

        private void Update()
        {
            CheckInputDevice();

            _inputDevice?.SetRotationAndScale();
            _inputDevice?.TryDisableIsDirectionSet();
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
            _inputDevice = _factory.Create(deviceName);

            RotationAmount = new ReadOnlyReactiveProperty<Vector3>(_inputDevice.Direction);
            ZScale = new ReadOnlyReactiveProperty<float>(_inputDevice.ZScale);
            IsDirectionSet = new ReadOnlyReactiveProperty<bool>(_inputDevice.IsDirectionSet);
            Angle = new ReadOnlyReactiveProperty<float>(_inputDevice.Angle);

            _inputChange.OnNext(Unit.Default);
        }
    }
}