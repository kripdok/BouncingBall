using BouncingBall.Game.Data;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device.Concrete
{
    public class KeyboardInputDevice : IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionActive { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> DistanceScale { get; private set; }
        public ReactiveProperty<float> Angle { get; private set; }

        private const float RotationSpeedMultiplier = 50f;
        private const float ScaleSpeedMultiplier = 5f;
        private const float CooldownDuration = 1f;

        private bool _isCooldown;
        private float _maxScale;

        public KeyboardInputDevice(GameDataManager gameDataManager)
        {
            _maxScale = gameDataManager.GameData.BallData.MaxSpeed;
            _isCooldown = false;
            IsDirectionActive = new ReactiveProperty<bool>();
            Direction = new ReactiveProperty<Vector3>();
            DistanceScale = new ReactiveProperty<float>();
            Angle = new ReactiveProperty<float>();
        }

        public void UpdateRotationAndScale()
        {
            if (_isCooldown)
                return;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            UpdateDirectionBasedOnInput(horizontal);
            UpdateScaleBasedOnInput(vertical);

            if (horizontal != 0 || vertical != 0)
            {
                IsDirectionActive.Value = true;
                CalculateDirectionVector();
            }
        }

        public void UpdateDirectionAndScale()
        {
            if (Input.GetButton("Jump") && IsDirectionActive.Value)
            {
                IsDirectionActive.Value = false;
                StartCooldown();
            }
        }

        public void Reset()
        {
            DistanceScale.Value = 0;
            Angle.Value = 0;
            Direction.Value = Vector3.zero;
            IsDirectionActive.Value = false;
        }

        private void UpdateDirectionBasedOnInput(float horizontalInput)
        {
            if (horizontalInput != 0f)
            {
                Angle.Value += RotationSpeedMultiplier * Time.deltaTime * Mathf.Sign(horizontalInput);
            }
        }

        private void UpdateScaleBasedOnInput(float verticalInput)
        {
            if (verticalInput != 0f)
            {
                float newScale = DistanceScale.Value + ScaleSpeedMultiplier * Time.deltaTime * Mathf.Sign(verticalInput);
                DistanceScale.Value = Mathf.Clamp(newScale, 0, _maxScale);
            }
        }

        private async void StartCooldown()
        {
            _isCooldown = true;
            await UniTask.WaitForSeconds(CooldownDuration);
            _isCooldown = false;
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
