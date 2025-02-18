using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] public Transform _holder;

    private float _speed;

    private BallData _ballData;
    private CompositeDisposable _inputDeviceDisposable;


    [Inject] private GameDataManager _gameDataManager;
    [Inject] private IInputManager _inputManager;

    public void Init()
    {
        _ballData = _gameDataManager.GameData.BallModel;
        _ballData.ReadPosition.Subscribe(SetPosition).AddTo(this);
        _ballData.ReadDirection.Subscribe(UpdateCameraPosition).AddTo(this);
        _inputManager.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
    }

    private void SubscribeToInput()
    {
        _inputDeviceDisposable?.Dispose();
        _inputDeviceDisposable = new();

        _inputManager.ZScale.Subscribe(UpdateSpeed).AddTo(_inputDeviceDisposable);
    }

    private void UpdateSpeed(float speed)
    {
        _speed = Mathf.Clamp(speed, 0, _ballData.MaxSpeed);
    }

    private void SetPosition(Vector3 position)
    {
        var newPosition = position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    private async void UpdateCameraPosition(Vector3 targetPosition)
    {
        Vector3 newCameraPosition = CalculateNewCameraPosition(targetPosition);
        await SmoothMoveCamera(newCameraPosition);
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
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            var t = elapsedTime / duration;
            _holder.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _holder.localPosition = targetPosition;
    }

}