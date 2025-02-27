using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.InputSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform _holder;
    [SerializeField] private float _moveDuration = 0.5f;

    [Inject] private GameDataManager _gameDataManager;
    [Inject] private IInputManager _inputManager;

    private CompositeDisposable _inputDeviceDisposable;
    private BallData _ballData;
    private float _yPosition;
    private float _speed;

    public void Init()
    {
        _yPosition = transform.position.y;
        _ballData = _gameDataManager.GameData.BallModel;
        Subscribe();
    }

    private void Subscribe()
    {
        _ballData.ReadPosition.Subscribe(SetPosition).AddTo(this);
        _ballData.ReadDirection.Subscribe(UpdateCameraPosition).AddTo(this);
        _inputManager.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);

        SubscribeToInput();
    }

    private void SetPosition(Vector3 position)
    {
        var newPosition = position;
        newPosition.y += _yPosition;
        transform.position = newPosition;
    }

    private void UpdateSpeed(float speed)
    {
        _speed = Mathf.Clamp(speed, 0, _ballData.MaxSpeed);
    }

    private async void UpdateCameraPosition(Vector3 targetPosition)
    {
        Vector3 newCameraPosition = CalculateNewCameraPosition(targetPosition);
        await SmoothMoveCamera(newCameraPosition);
    }

    private void SubscribeToInput()
    {
        _inputDeviceDisposable?.Dispose();
        _inputDeviceDisposable = new();

        _inputManager.ZScale.Subscribe(UpdateSpeed).AddTo(_inputDeviceDisposable);
    }

    private Vector3 CalculateNewCameraPosition(Vector3 targetPosition)
    {
        var targetDirection = targetPosition.normalized;

        float speedFactor = _speed / _ballData.MaxSpeed;
        Vector3 newPosition = targetDirection * speedFactor;
        newPosition.y = _holder.localPosition.y;

        return newPosition;
    }

    private async UniTask SmoothMoveCamera(Vector3 targetPosition)
    {
        Vector3 startPosition = _holder.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            var t = elapsedTime / _moveDuration;
            _holder.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _holder.localPosition = targetPosition;
    }
}