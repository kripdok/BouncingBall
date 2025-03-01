using BouncingBall.Game.Data;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace BouncingBall.InputSystem.Device
{
    public class PlayerInputSimulator : IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> ZScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private float _maxScale;

        private bool _isWork;
        private float _durationOfSettingValues = 2f;

        public PlayerInputSimulator(GameDataManager gameDataManager)
        {
            _maxScale = gameDataManager.GameData.BallModel.MaxSpeed;
            IsDirectionSet = new ReactiveProperty<bool>(false);
            Direction = new ReactiveProperty<Vector3>(Vector3.zero);
            ZScale = new ReactiveProperty<float>(0f);
            Angle = new();

            Simulate();
        }

        public async void Simulate()
        {
            _isWork = true;

            while (_isWork)
            {
                ZScale.Value = 0;
                Angle.Value = 0;

                var newZScale = Random.Range(1, _maxScale);
                var newAngle = Random.Range(0, 360);

                float elapsedTime = 0f;
                await UniTask.WaitForSeconds(2);
                IsDirectionSet.Value = true;

                while (elapsedTime < _durationOfSettingValues)
                {
                    float lerpT = elapsedTime / _durationOfSettingValues;
                    ZScale.Value = Mathf.Lerp(ZScale.Value, newZScale, lerpT);
                    Angle.Value = Mathf.Lerp(Angle.Value, newAngle, lerpT);
                    UpdateDirection();

                    elapsedTime += Time.deltaTime;
                    await UniTask.Yield();

                    if (!_isWork)
                        break;
                }

                if (!_isWork)
                    break;

                ZScale.Value = newZScale;
                Angle.Value = newAngle;
                UpdateDirection();

                await UniTask.WaitForSeconds(0.5f);

                IsDirectionSet.Value = false;
            }
        }

        public void Disable()
        {
            _isWork = false;

        }

        public void SetRotationAndScale()
        {

        }

        public void TryDisableIsDirectionSet()
        {
        }

        private void UpdateDirection()
        {
            float angleInRadians = (Angle.Value + 90) * Mathf.Deg2Rad;
            float distance = ZScale.Value;

            float x = distance * Mathf.Cos(angleInRadians) * -1;
            float z = distance * Mathf.Sin(angleInRadians);

            var direction = new Vector3(x, 0, z);
            Direction.Value = direction.normalized;
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
