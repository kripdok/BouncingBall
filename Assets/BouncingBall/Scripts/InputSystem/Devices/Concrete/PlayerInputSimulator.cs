using BouncingBall.Game.Data;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device.Concrete
{
    public class PlayerInputSimulator : IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionActive { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> DistanceScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private const float SimulationDuration = 2f;
        private const float PauseDuration = 0.5f;

        private float _maxScale;
        private bool _isSimulationActive;

        public PlayerInputSimulator(GameDataProvider gameDataManager)
        {
            _maxScale = gameDataManager.GameData.BallData.MaxSpeed;
            IsDirectionActive = new ReactiveProperty<bool>(false);
            Direction = new ReactiveProperty<Vector3>(Vector3.zero);
            DistanceScale = new ReactiveProperty<float>(0f);
            Angle = new ReactiveProperty<float>();

            StartSimulation();
        }

        public async void StartSimulation()
        {
            _isSimulationActive = true;

            while (_isSimulationActive)
            {
                ResetValues();

                float newZScale = GenerateRandomDistanceScale();
                float newAngle = GenerateRandomAngle();

                await UniTask.WaitForSeconds(2);
                IsDirectionActive.Value = true;

                await LerpValuesOverTime(newZScale, newAngle);

                if (!_isSimulationActive)
                    break;

                ApplyFinalValues(newZScale, newAngle);

                await UniTask.WaitForSeconds(PauseDuration);

                IsDirectionActive.Value = false;
            }
        }

        public void StopSimulation()
        {
            _isSimulationActive = false;
        }

        public void UpdateRotationAndScale()
        {
            return;
        }

        public void UpdateDirectionAndScale()
        {
            return;
        }

        public void Reset()
        {
            DistanceScale.Value = 0;
            Angle.Value = 0;
            Direction.Value = Vector3.zero;
            IsDirectionActive.Value = false;
        }

        private void ResetValues()
        {
            DistanceScale.Value = 0;
            Angle.Value = 0;
        }

        private float GenerateRandomDistanceScale()
        {
            return Random.Range(1, _maxScale);
        }

        private float GenerateRandomAngle()
        {
            return Random.Range(0, 360);
        }

        private async UniTask LerpValuesOverTime(float targetZScale, float targetAngle)
        {
            float elapsedTime = 0f;

            while (elapsedTime < SimulationDuration)
            {
                if (!_isSimulationActive)
                    break;

                float lerpT = elapsedTime / SimulationDuration;
                DistanceScale.Value = Mathf.Lerp(DistanceScale.Value, targetZScale, lerpT);
                Angle.Value = Mathf.Lerp(Angle.Value, targetAngle, lerpT);
                CalculateDirectionVector();

                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private void ApplyFinalValues(float targetZScale, float targetAngle)
        {
            DistanceScale.Value = targetZScale;
            Angle.Value = targetAngle;
            CalculateDirectionVector();
        }

        private void CalculateDirectionVector()
        {
            float angleInRadians = (Angle.Value + 90) * Mathf.Deg2Rad;
            float distance = DistanceScale.Value;

            float x = distance * Mathf.Cos(angleInRadians) * -1;
            float z = distance * Mathf.Sin(angleInRadians);

            Direction.Value = new Vector3(x, 0, z).normalized;
        }
    }
}
