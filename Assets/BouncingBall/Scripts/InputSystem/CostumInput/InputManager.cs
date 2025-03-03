using BouncingBall.InputSystem.Controller;
using BouncingBall.InputSystem.Device;
using BouncingBall.Utilities;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem
{
    public class InputManager : IInputInteractivityChanger, IInputManager, IPausable
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

        private bool _isPouse;

        public InputManager(InputDevicePool factory)
        {
            _factory = factory;
            _currentInputDeviceName = InputDeviceTag.Keyboard;
        }

        public void EnableInput()
        {
            Debug.Log("Enable input");
            _disposable = new CompositeDisposable();
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposable);
            Resume();

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
            if (_inputDevice is PlayerInputSimulator input)
            {
                input.Disable();
                DisableInput();
            }
        }

        private void Update()
        {
            if (_isPouse)
                return;

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
            Debug.Log("Сейчас влючен дивайс"+_currentInputDeviceName);
        }

        public void Pause()
        {
            _isPouse = true;
        }

        public void Resume()
        {
            _isPouse = false;
        }
    }
}