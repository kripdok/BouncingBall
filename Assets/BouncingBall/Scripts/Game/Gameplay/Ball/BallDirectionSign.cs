using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class BallDirectionSign : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float scaleSpeed = 5f;

        private IInputManager _inputController;
        private CompositeDisposable _inputDeviceDisposable;

        [Inject]
        public void Constructor(IInputManager inputController)
        {
            _inputController = inputController;
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;

            _inputController.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputController.ZScale.Subscribe(UpdateScale).AddTo(_inputDeviceDisposable);
            _inputController.RotationAmount.Subscribe(UpdateRotation).AddTo(_inputDeviceDisposable);
            _inputController.IsDirectionSet.Skip(1).Subscribe(Punch2).AddTo(_inputDeviceDisposable);
        }

        private void UpdateRotation(Vector3 direction)
        {
            // Плавно поворачиваем объект к целевому направлению
            transform.rotation = Quaternion.Euler(direction);
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
