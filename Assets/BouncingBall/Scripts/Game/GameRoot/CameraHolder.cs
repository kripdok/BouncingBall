using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.InputSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.GameRoot
{
    public class CameraHolder : MonoBehaviour
    {
        private const float MinSpeed = 0f;

        [SerializeField] private Transform _holder;
        [SerializeField] private float _moveDuration = 0.5f;

        [Inject] private GameDataManager _gameDataManager;
        [Inject] private IInputManager _inputManager;

        private CompositeDisposable _inputDeviceDisposable;
        private BallData _ballData;
        private float _initialYPosition;
        private float _speed;
        private bool _isWork;

        public void Init()
        {
            _isWork = true;
            _initialYPosition = transform.position.y;
            _ballData = _gameDataManager.GameData.BallData;

            Subscribe();
        }

        private void OnDestroy()
        {
            _isWork = false;
        }

        private void Subscribe()
        {
            SubscribeToInputEventsAndBallData();
            SubscribeToInputDevice();
        }

        private void SubscribeToInputEventsAndBallData()
        {
            _ballData.Position.Subscribe(SetPosition).AddTo(this);
            _ballData.Direction.Subscribe(MoveCameraToTargetPosition).AddTo(this);
            _inputManager.InputChange.Subscribe(_ => SubscribeToInputDevice()).AddTo(this);
        }
        private void SetPosition(Vector3 position)
        {
            var newPosition = position;
            newPosition.y += _initialYPosition;
            transform.position = newPosition;
        }

        private void UpdateCameraSpeed(float speed)
        {
            _speed = Mathf.Clamp(speed, MinSpeed, _ballData.MaxSpeed);
        }

        private async void MoveCameraToTargetPosition(Vector3 targetPosition)
        {
            Vector3 newCameraPosition = CalculateTargetCameraPosition(targetPosition);
            await SmoothlyMoveCamera(newCameraPosition);
        }

        private void SubscribeToInputDevice()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new CompositeDisposable();

            _inputManager.DistanceScale?.Subscribe(UpdateCameraSpeed).AddTo(_inputDeviceDisposable);
        }

        private Vector3 CalculateTargetCameraPosition(Vector3 targetPosition)
        {
            var targetDirection = targetPosition.normalized;

            float speedFactor = _speed / _ballData.MaxSpeed;
            Vector3 newPosition = targetDirection * speedFactor;
            newPosition.y = _holder.localPosition.y;

            return newPosition;
        }

        private async UniTask SmoothlyMoveCamera(Vector3 targetPosition)
        {
            Vector3 startPosition = _holder.localPosition;
            float elapsedTime = 0f;

            while (elapsedTime < _moveDuration && _isWork)
            {
                float lerpT = elapsedTime / _moveDuration;
                _holder.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            _holder.localPosition = targetPosition;
        }
    }
}