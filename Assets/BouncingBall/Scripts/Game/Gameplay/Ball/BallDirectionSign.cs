using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.Game.Data;
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

        [Inject]
        public void Constructor(GameDataManager gameDataManager,IInputManager inputController)
        {
            _inputController = inputController;
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
            gameDataManager.GameData.BallModel.Position.Subscribe(x => transform.position = x).AddTo(this);
            _inputController.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputController.ZScale.Subscribe(UpdateScale).AddTo(_inputDeviceDisposable);
            _inputController.Angle.Subscribe(UpdateRotation).AddTo(_inputDeviceDisposable);
            _inputController.IsDirectionSet.Skip(1).Subscribe(Punch2).AddTo(_inputDeviceDisposable);
        }

        private void UpdateRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private void UpdateScale(float zScale)
        {
            Vector3 newScale = transform.localScale;
            newScale.z = Mathf.Clamp(zScale, 0, 3f); 
            transform.localScale = newScale;
        }

        private void Punch2(bool flag)
        {
            gameObject.SetActive(flag);

            if (flag == false)
            {
                Debug.Log("Произошел удар");
            }
        }


        private void OnDestroy()
        {
            _inputDeviceDisposable?.Dispose();
        }
    }
}
