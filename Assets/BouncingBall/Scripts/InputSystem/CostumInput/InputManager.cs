using BouncingBall.InputSystem.Controller;
using UniRx;
using UnityEngine;

public class InputManager : IPointingDirection, IInputInteractivityChanger
{
    private IInputDevice _inputDivace;

    public ReadOnlyReactiveProperty<Vector2> PointerLocation { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; private set; }

    public ISubject<Vector2> Position => _pointerPosition;

    private readonly ReactiveProperty<Vector2> _pointerLocation = new();
    private readonly ReactiveProperty<bool> _isDirectionSet = new();

    private Subject<Vector2> _pointerPosition = new();

    private CompositeDisposable _disposable;

    public InputManager()
    {
        _inputDivace = new KeyboardInputDevice();

        IsDirectionSet = new ReadOnlyReactiveProperty<bool>(_isDirectionSet);
        PointerLocation = new ReadOnlyReactiveProperty<Vector2>(_pointerLocation);
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
        _pointerPosition.OnNext(_inputDivace.GetMovementInput());
       // _pointerLocation.SetValueAndForceNotify(_inputDivace.GetMovementInput());
        _isDirectionSet.SetValueAndForceNotify(_inputDivace.GetActionInput());
    }
}
