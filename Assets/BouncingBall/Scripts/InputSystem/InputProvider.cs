using BouncingBall.InputSystem.Device;
using BouncingBall.InputSystem.Device.Concrete;
using BouncingBall.Utilities;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem
{
    public class InputProvider : IInputInteractivityChanger, IInputProvider, IPausable
    {
        public ReadOnlyReactiveProperty<Vector3> Direction { get; private set; }
        public ReadOnlyReactiveProperty<float> DistanceScale { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDirectionActive { get; private set; }
        public ReadOnlyReactiveProperty<float> Angle { get; private set; }

        private Subject<Unit> _inputChange = new Subject<Unit>();
        private CompositeDisposable _disposable;
        private InputDevicePool _factory;
        private IInputDevice _inputDevice;
        private InputDeviceTag _currentInputDeviceName;

        public ISubject<Unit> InputChange => _inputChange;

        private bool _isPaused;

        public InputProvider(InputDevicePool factory)
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
            Direction?.Dispose();
            DistanceScale?.Dispose();
            IsDirectionActive?.Dispose();
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

        public void EnableSimulatedInput()
        {
            InitializeInputDevice(InputDeviceTag.Simulator);

            if (_inputDevice is PlayerInputSimulator input)
            {
                input.StartSimulation();
            }
        }

        public void DisableSimulatedInput()
        {
            if (_inputDevice is PlayerInputSimulator input)
            {
                input.StopSimulation();
                DisableInput();
            }
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        private void Update()
        {
            if (_isPaused)
                return;

            UpdateInputDeviceBasedOnInput();

            _inputDevice?.UpdateRotationAndScale();
            _inputDevice?.UpdateDirectionAndScale();
        }

        private void UpdateInputDeviceBasedOnInput()
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

            Direction = new ReadOnlyReactiveProperty<Vector3>(_inputDevice.Direction);
            DistanceScale = new ReadOnlyReactiveProperty<float>(_inputDevice.DistanceScale);
            IsDirectionActive = new ReadOnlyReactiveProperty<bool>(_inputDevice.IsDirectionActive);
            Angle = new ReadOnlyReactiveProperty<float>(_inputDevice.Angle);

            _inputChange.OnNext(Unit.Default);
        }
    }
}