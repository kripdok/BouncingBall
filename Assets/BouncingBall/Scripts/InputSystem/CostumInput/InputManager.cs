using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.InputSystem.Controller;
using UniRx;
using UnityEngine;

public class InputManager : IPointingDirection, IInputInteractivityChanger, ITestInputManager
{
    private KeyboardInputDevice _testInputDevice;

    public ReadOnlyReactiveProperty<float> RotationAmount { get; private set; }
    public ReadOnlyReactiveProperty<float> ZScale { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsDirectionSet2 { get; private set; }






    public ReadOnlyReactiveProperty<Vector2> PointerLocation { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; private set; }

    public ISubject<Vector2> Position => _pointerPosition;

    private readonly ReactiveProperty<Vector2> _pointerLocation = new();
    private readonly ReactiveProperty<bool> _isDirectionSet = new();

    private Subject<Vector2> _pointerPosition = new();

    private CompositeDisposable _disposable;

    public InputManager()
    {
        _testInputDevice = new KeyboardInputDevice();

        IsDirectionSet = new ReadOnlyReactiveProperty<bool>(_isDirectionSet);
        PointerLocation = new ReadOnlyReactiveProperty<Vector2>(_pointerLocation);

        RotationAmount = new(_testInputDevice.RotationAmount);
        ZScale = new(_testInputDevice.ZScale);
        IsDirectionSet2 = new(_testInputDevice.IsDirectionSet);
    }

    public void EnableInput()
    {
        _disposable = new();
        Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_disposable);
    }

    public void DisableInput()
    {
        _disposable.Dispose();
    }

    private void Update()
    {
        // _pointerPosition.OnNext(_testInputDevice.GetMovementInput());
        //// _pointerLocation.SetValueAndForceNotify(_inputDivace.GetMovementInput());
        // _isDirectionSet.SetValueAndForceNotify(_testInputDevice.GetActionInput());

        _testInputDevice.SetRotationAndScale();
        _testInputDevice.TryDisableIsDirectionSet();
    }
}
