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

        private IPointingDirection _inputController;
        private CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public void Constructor(IPointingDirection inputController)
        {
            _inputController = inputController;
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;

            // Подписка на изменение направления
            _inputController.Position
                .Subscribe(UpdateDirectionSign)
                .AddTo(_disposables);


            _inputController.IsDirectionSet.Subscribe(_ => Punch()).AddTo(_disposables);
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

