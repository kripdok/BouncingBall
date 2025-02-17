using BouncingBall.Game.Data;
using UniRx;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public class TouchpadInputDevice : IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> ZScale { get; private set; }

        private Plane _plane;
        private Vector3 _ballPosition;
        private GameDataManager _gameDataManager;

        public TouchpadInputDevice(GameDataManager gameDataManager)
        {
            IsDirectionSet = new ReactiveProperty<bool>();
            Direction = new ReactiveProperty<Vector3>();
            ZScale = new ReactiveProperty<float>();

            _plane = new Plane(Vector3.up, new Vector3(0, 0.5f, 0));
            _gameDataManager = gameDataManager;
        }

        public void SetRotationAndScale()
        {
            IsDirectionSet.Value = Input.touchCount > 0;
        }

        public void TryDisableIsDirectionSet()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                _ballPosition = _gameDataManager.GameData.BallModel.ReadPosition.Value;

                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (_plane.Raycast(ray, out float distance))
                {
                    Vector3 position = ray.GetPoint(distance);
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
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Direction.Value = new Vector3(0, angle, 0);
            }
        }

        private void CalculationScaleZ(Vector3 position)
        {
            ZScale.Value = Vector3.Distance(_ballPosition, position);

        }

        public void SetTest()
        {
           
        }
    }
}
