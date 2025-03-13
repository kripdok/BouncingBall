using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device.Concrete
{
    public class MouseInputDevice : IInputDevice, IControllable
    {
        public ReactiveProperty<bool> IsDirectionActive { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> DistanceScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private Plane _plane;
        private Vector3 _ballPosition;
        private GameDataProvider _gameDataManager;

        public bool IsControllable { get; private set; }

        public MouseInputDevice(GameDataProvider gameDataManager)
        {
            IsDirectionActive = new ReactiveProperty<bool>();
            Direction = new ReactiveProperty<Vector3>();
            DistanceScale = new ReactiveProperty<float>();
            Angle = new ReactiveProperty<float>();

            _plane = new Plane(Vector3.up, Vector3.zero);
            _gameDataManager = gameDataManager;
            _gameDataManager.GameData.BallData.Position.Subscribe(SetBallPositionAndPlanePoint);
            IsControllable = false;
        }

        public void Reset()
        {
            DistanceScale.Value = 0;
            Angle.Value = 0;
            Direction.Value = Vector3.zero;
            IsDirectionActive.Value = false;
        }

        public void EnableControllable()
        {
            IsControllable = true;
        }

        public void UpdateRotationAndScale()
        {
            if (IsControllable)
            {
                IsDirectionActive.Value = Input.GetMouseButton(0);
                IsControllable = IsDirectionActive.Value;
            }
        }

        public void UpdateDirectionAndScale()
        {
            if (IsControllable)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_plane.Raycast(ray, out float distance))
                {
                    var position = ray.GetPoint(distance);
                    CalculateDistanceScale(position);
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

        private void CalculateDistanceScale(Vector3 position)
        {
            DistanceScale.Value = Vector3.Distance(_ballPosition, position);
        }

        private void SetBallPositionAndPlanePoint(Vector3 position)
        {
            _ballPosition = position;
            _plane.SetNormalAndPosition(Vector3.up, position);
        }
    }
}