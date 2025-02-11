using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.InputSystem.Controller;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class BallDirectionSign : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float scaleSpeed = 5f;

        private ITestInputManager _inputController;
        private CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public void Constructor(ITestInputManager inputController)
        {
            _inputController = inputController;
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;

            // Подписка на изменение направления
            //_inputController.Position
            //    .Subscribe(UpdateDirectionSign)
            //    .AddTo(_disposables);


            //_inputController.IsDirectionSet.Subscribe(_ => Punch()).AddTo(_disposables);

            _inputController.ZScale.Skip(1).Subscribe(UpdateScale).AddTo(_disposables);
            _inputController.RotationAmount.Skip(1).Subscribe(UpdateRotation).AddTo(_disposables);
            _inputController.IsDirectionSet2.Skip(1).Subscribe(Punch2).AddTo(_disposables);
        }

        private void UpdateRotation(float yAngle)
        {
            transform.Rotate(0, yAngle, 0);
        }

        private void UpdateScale(float zScale)
        {
            Vector3 newScale = transform.localScale;
            newScale.z += zScale;
            newScale.z = Mathf.Clamp(newScale.z, -3, 3f);
            transform.localScale = newScale;
        }

        private void Punch2(bool flag)
        {
            gameObject.SetActive(flag);

            if(flag == false)
            {
                Debug.Log("Произошел удар");
            }
        }

        private void UpdateDirectionSign(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            gameObject.SetActive(true);
            Debug.Log(direction);

            if (direction.x != 0)
            {
                float rotationAmount = rotationSpeed * Time.deltaTime * (direction.x > 0 ? 1 : -1);
                transform.Rotate(0, rotationAmount, 0);
            }

            if (direction.y != 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.z += scaleSpeed * Time.deltaTime * (direction.y > 0 ? 1 : -1);
                newScale.z = Mathf.Clamp(newScale.z, -3, 3f);
                transform.localScale = newScale;
            }
        }

        private void Punch()
        {
            if (gameObject.activeSelf == false)
                return;

            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
            Debug.Log("Произошел удар");
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}

