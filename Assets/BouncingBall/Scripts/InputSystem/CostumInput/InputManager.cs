using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.InputSystem.Controller;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : IInputInteractivityChanger, IInputManager 
{
    public ReadOnlyReactiveProperty<Vector3> RotationAmount { get; private set; }
    public ReadOnlyReactiveProperty<float> ZScale { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReadOnlyReactiveProperty<float> Angle { get; private set; }
    public ISubject<Unit> InputChange => _inputChange;


    private Subject<Unit> _inputChange = new();
    private CompositeDisposable _disposable;
    private InputDevicePool _factory;
    private IInputDevice _testInputDevice;
    private InputDeviceName _currentInputDeviceName;

    public InputManager(InputDevicePool factory)
    {
        _factory = factory;
        InitializeInputDevice(InputDeviceName.Keyboard);
    }

    public void EnableInput()
    {
        _disposable = new CompositeDisposable();
        Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposable);
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
            controllableDevice.EnableControllable(); // Вызов метода включения управляемости
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
        InputDeviceName newInputDeviceName;

        if (Input.GetMouseButton(0))
        {
            newInputDeviceName = InputDeviceName.Mouse;
        }
        else if (Input.anyKey)
        {
            newInputDeviceName = InputDeviceName.Keyboard;
        }
        else if (Input.touchCount > 0)
        {
            newInputDeviceName = InputDeviceName.Touchpad;
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

    private void InitializeInputDevice(InputDeviceName deviceName)
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