using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.InputSystem;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.BallEntity
{
    public class BallDirectionSign : MonoBehaviour
    {
        [Inject] private IInputProvider _inputController;
        private CompositeDisposable _inputDeviceDisposable;
        private BallData _ballData;

        [Inject]
        private void Constructor(GameDataProvider gameDataManager)
        {
            _ballData = gameDataManager.GameData.BallData;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;

            Subscribe();
        }

        private void OnDestroy()
        {
            _inputDeviceDisposable?.Dispose();
        }

        private void Subscribe()
        {
            _ballData.Position.Subscribe(x => transform.position = x).AddTo(this);
            _inputController.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);

            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputController.DistanceScale?.Subscribe(UpdateScale).AddTo(_inputDeviceDisposable);
            _inputController.Angle?.Subscribe(UpdateRotation).AddTo(_inputDeviceDisposable);
            _inputController.IsDirectionActive?.Skip(1).Subscribe(flag => gameObject.SetActive(flag)).AddTo(_inputDeviceDisposable);
        }

        private void UpdateRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private void UpdateScale(float zScale)
        {
            Vector3 newScale = transform.localScale;
            newScale.z = Mathf.Clamp(zScale, 0, _ballData.MaxSpeed);
            transform.localScale = newScale;
        }
    }
}
