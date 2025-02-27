using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;
namespace BouncingBall.InputSystem.Device
{
    public class MouseInputDevice : IInputDevice, IControllable
    {
        public ReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> ZScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private Plane _plane;
        private Vector3 _ballPosition;
        private GameDataManager _gameDataManager;

        public bool IsControllable { get; private set; }

        public MouseInputDevice(GameDataManager gameDataManager)
        {
            IsDirectionSet = new();
            Direction = new();
            ZScale = new();
            Angle = new();

            _plane = new(Vector3.up, Vector3.zero);
            _gameDataManager = gameDataManager;
            _gameDataManager.GameData.BallModel.ReadPosition.Subscribe(SetBallPositionAndPlanePoint);
            IsControllable = false;
        }

        public void EnableControllable()
        {
            IsControllable = true;
        }

        public void SetRotationAndScale()
        {
            if (IsControllable)
            {
                IsDirectionSet.Value = Input.GetMouseButton(0);
                IsControllable = IsDirectionSet.Value;
            }
        }

        public void TryDisableIsDirectionSet()
        {
            if (IsControllable)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_plane.Raycast(ray, out float distance))
                {
                    var position = ray.GetPoint(distance);
                    CalculationScaleZ(position);
                    CalculateDirection(position);
                }
            }
        }

        private void CalculateDirection(Vector3 position)
        {
            Vector3 direction = position - _ballPosition;

            if (direction != Vector3.zero)
            {
                Direction.Value = new Vector3(direction.x, 0, direction.z).normalized;
                Angle.Value = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            }
        }

        private void CalculationScaleZ(Vector3 position)
        {
            ZScale.Value = Vector3.Distance(_ballPosition, position);
        }

        private void SetBallPositionAndPlanePoint(Vector3 position)
        {
            _ballPosition = position;
            _plane.SetNormalAndPosition(Vector3.up, position);
        }
    }
}

