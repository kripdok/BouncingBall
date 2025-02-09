using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Controller
{
    public class InputController : IPointingDirection, IInputInteractivityChanger
    {
        public ReadOnlyReactiveProperty<Vector3> PointerLocation { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; private set; }

        private readonly ReactiveProperty<Vector3> _pointerLocation;
        private readonly ReactiveProperty<bool> _isDirectionSet;
        private readonly InputSystemActions _inputActions;

        public InputController(InputSystemActions inputActions)
        {
            _inputActions = inputActions;

            _isDirectionSet = new ReactiveProperty<bool>();
            IsDirectionSet = new ReadOnlyReactiveProperty<bool>(_isDirectionSet);

            _pointerLocation = new ReactiveProperty<Vector3>();
            PointerLocation = new ReadOnlyReactiveProperty<Vector3>(_pointerLocation);

            _inputActions.Player.Attack.started += cnt => StartMove();
            _inputActions.Player.Attack.canceled += cnt => StopMove();
        }

        public void EnableInput()
        {
            _inputActions.Enable();
        }

        public void DisableInput()
        {
            _inputActions.Disable();
        }

        private async void StartMove()
        {
            _isDirectionSet.Value = true;
            await Move();

            //Подает сигнал для включения указателя
        }

        private void StopMove()
        {
            _isDirectionSet.Value = false;
        }

        private async UniTask Move()
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            do
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distance))
                {
                    _pointerLocation.Value = ray.GetPoint(distance);
                }

                await UniTask.Yield();
            }
            while (_isDirectionSet.Value);
        }


    }
}
