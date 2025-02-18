using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class BallDirectionSign : MonoBehaviour
    {
        private IInputManager _inputController;
        private CompositeDisposable _inputDeviceDisposable;
        private BallData _ballData;

        [Inject]
        public void Constructor(GameDataManager gameDataManager,IInputManager inputController)
        {
            _inputController = inputController;
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
            _ballData = gameDataManager.GameData.BallModel;
            _ballData.Position.Subscribe(x => transform.position = x).AddTo(this);
            _inputController.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputController.ZScale.Subscribe(UpdateScale).AddTo(_inputDeviceDisposable);
            _inputController.Angle.Subscribe(UpdateRotation).AddTo(_inputDeviceDisposable);
            _inputController.IsDirectionSet.Skip(1).Subscribe(flag => gameObject.SetActive(flag)).AddTo(_inputDeviceDisposable);
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

        private void OnDestroy()
        {
            _inputDeviceDisposable?.Dispose();
        }
    }
}
