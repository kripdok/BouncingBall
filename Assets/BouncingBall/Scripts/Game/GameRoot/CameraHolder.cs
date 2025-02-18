using BouncingBall.Game.Data;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] public Transform _holder;

    private float _speed;

    [Inject] private GameDataManager _gameDataManager;

    public void Init()
    {
        _gameDataManager.GameData.BallModel.ReadPosition.Subscribe(SetPosition).AddTo(this);
        _gameDataManager.GameData.BallModel.ReadDirection.Subscribe(SetCameraPosition).AddTo(this);
    }


    private void SetPosition(Vector3 position)
    {
        var newPosition = position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    private async void SetCameraPosition(Vector3 position)
    {
        Debug.Log(position);
        var newPosition = position.normalized;
        newPosition.y = _holder.localPosition.y;

        Vector3 startPosition = _holder.localPosition;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            _holder.localPosition = Vector3.Lerp(startPosition, newPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _holder.localPosition = newPosition;
    }

}