using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BouncingBall.Scripts.InputSystem.Controller
{
    public class InputController
    {
        public ReadOnlyReactiveProperty<Vector3> PointerLocation;
        public ReadOnlyReactiveProperty<bool> IsWork;

        private readonly ReactiveProperty<Vector3> _pointerLocation;
        private readonly ReactiveProperty<bool> _isWork;
        private readonly InputSystemActions _inputActions;

        //TODO - подумать где будет отключаться управление. Для этого нужен Интерфейс?
        public InputController(InputSystemActions inputActions)
        {
            _inputActions = inputActions;

            _isWork = new ReactiveProperty<bool>();
            IsWork = new ReadOnlyReactiveProperty<bool>(_isWork);

            _pointerLocation = new ReactiveProperty<Vector3>();
            PointerLocation = new ReadOnlyReactiveProperty<Vector3>(_pointerLocation);

            _inputActions.Player.Attack.started += cnt => StartMove();
            _inputActions.Player.Attack.canceled += cnt => StopMove();

            PointerLocation.Subscribe(vector => Debug.Log("МОй код " + vector));
        }

        private async void StartMove()
        {
            _isWork.Value = true;
            await Move();

            //Подает сигнал для включения указателя
        }

        private void StopMove()
        {
            _isWork.Value = false;
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
            while (_isWork.Value);
        }
    }
}
