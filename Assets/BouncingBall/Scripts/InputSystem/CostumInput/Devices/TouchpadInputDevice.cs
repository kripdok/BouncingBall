using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device
{
    public class TouchpadInputDevice : IInputDevice, IControllable
    {
        public ReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> ZScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private Plane _plane;
        private Vector3 _ballPosition;
        private GameDataManager _gameDataManager;

        public bool IsControllable { get; private set; }

        public TouchpadInputDevice(GameDataManager gameDataManager)
        {
            IsDirectionSet = new();
            Direction = new();
            ZScale = new();
            Angle = new();

            _plane = new(Vector3.up, Vector3.zero);
            _gameDataManager = gameDataManager;
            _gameDataManager.GameData.BallData.Position.Subscribe(SetBallPositionAndPlanePoint);
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
                IsDirectionSet.Value = Input.touchCount > 0;
                IsControllable = IsDirectionSet.Value;
            }
        }

        public void TryDisableIsDirectionSet()
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
                        CalculationScaleZ(worldPosition);
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

                Debug.Log("Установленj направление" + Direction.Value);
                Debug.Log("Установлен угол" + Angle.Value);
            }
        }

        private void CalculationScaleZ(Vector3 position)
        {
            ZScale.Value = Vector3.Distance(_ballPosition, position);
            Debug.Log("Установлен скейл" + ZScale.Value);
        }

        private void SetBallPositionAndPlanePoint(Vector3 position)
        {
            _ballPosition = position;
            _plane.SetNormalAndPosition(Vector3.up, position);
        }

        public void Reset()
        {
            ZScale.Value = 0;
            Angle.Value = 0;
            Direction.Value = Vector3.zero;
            IsDirectionSet.Value = false;
        }
    }
}
