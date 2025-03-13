using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device.Concrete
{
    public class TouchpadInputDevice : IInputDevice, IControllable
    {
        public ReactiveProperty<bool> IsDirectionActive { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> DistanceScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private Plane _plane;
        private Vector3 _ballPosition;
        private GameDataProvider _gameDataManager;

        public bool IsControllable { get; private set; }

        public TouchpadInputDevice(GameDataProvider gameDataManager)
        {
            IsDirectionActive = new();
            Direction = new();
            DistanceScale = new();
            Angle = new();

            _plane = new(Vector3.up, Vector3.zero);
            _gameDataManager = gameDataManager;
            _gameDataManager.GameData.BallData.Position.Subscribe(UpdateBallPositionAndPlane);
            IsControllable = false;
        }

        public void EnableControllable()
        {
            IsControllable = true;
        }

        public void UpdateRotationAndScale()
        {
            if (IsControllable)
            {
                IsDirectionActive.Value = Input.touchCount > 0;
                IsControllable = IsDirectionActive.Value;
            }
        }

        public void UpdateDirectionAndScale()
        {
            if (IsControllable)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.touches[0];
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 touchPosition = touch.position;
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
                        CalculateDistanceScale(worldPosition);
                        CalculateDirection(worldPosition);
                    }
                }
                else
                {
                    Reset();
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

        private void UpdateBallPositionAndPlane(Vector3 position)
        {
            _ballPosition = position;
            _plane.SetNormalAndPosition(Vector3.up, position);
        }

        public void Reset()
        {
            DistanceScale.Value = 0;
            Angle.Value = 0;
            Direction.Value = Vector3.zero;
            IsDirectionActive.Value = false;
        }
    }
}
