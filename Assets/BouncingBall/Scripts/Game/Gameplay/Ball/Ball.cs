using BouncingBall.Scripts.InputSystem.Controller;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.BallSystem
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Transform _transformBody;

        [Header("Ball Settings")]
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float rotationSpeedFactor = 1f;

        private IPointingDirection _pointingDirection;
        private IDisposable _subscriptions;
        private BallModel _model;

        [Inject]
        public void Constructor(BallModel model)
        {
            _model = model;
        }

        public void SetPointingDirection(IPointingDirection pointingDirection)
        {
            _subscriptions?.Dispose();

            _pointingDirection = pointingDirection;
            _subscriptions = _pointingDirection.IsDirectionSet.Skip(1).Where(t => t == false).Subscribe(_ => Test());
        }
        private void Test()
        {
            Accelerate(_pointingDirection.PointerLocation.Value);
        }


        public float rotationSpeed = 5f; // Скорость вращения
        public float decelerationRate = 1f; // Скорость замедления

        public async UniTask Accelerate(Vector3 targetPoint)
        {
            // Получаем текущее положение шара
            var position = transform.position;

            // Вычисляем направление к целевой точке
            Vector3 direction = (targetPoint - position).normalized;
            direction.y = 0;
            var speed = maxSpeed;

            while (speed >= 0)
            {
                // Обновляем позицию мяча
                position += direction * speed * Time.deltaTime;
                transform.position = position;

                _model.Position.Value = transform.position;

                // Вращаем мяч по оси X
                float rotationSpeed = speed * rotationSpeedFactor;
                _transformBody.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

                // Уменьшаем скорость
                speed -= 0.1f;

                await UniTask.Yield();
            }

        }


    }
}


//// Асинхронный метод для вращения шара
//public async UniTask RotateBall(Vector3 direction)
//{
//    // Преобразуем направление в кватернион
//    Quaternion targetRotation = Quaternion.LookRotation(direction);
//    Quaternion currentRotation = transform.rotation;

//    // Устанавливаем начальную скорость вращения
//    float currentSpeed = rotationSpeed;

//    while (currentSpeed > 0)
//    {
//        // Обновляем текущую ротацию
//        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, currentSpeed * Time.deltaTime);
//        _transformBody.rotation = currentRotation;

//        // Уменьшаем скорость вращения
//        currentSpeed -= decelerationRate * Time.deltaTime;

//        // Ждем следующий кадр
//        await UniTask.Yield();
//    }

//    // Убедитесь, что шар завершил вращение в финальной позиции
//    _transformBody.rotation = targetRotation;
//}


//public async UniTask Accelerate(Vector3 targetPoint)
//{
//    var position = transform.position;

//    Vector3 moveDirection = (targetPoint - position).normalized;
//    moveDirection.y = 0;
//    var speed = maxSpeed;

//    while (speed >= 0)
//    {
//        position = Move(position, moveDirection, speed);
//        transform.position = position;

//        // Вращаем мяч
//        RotateBall(moveDirection, speed);

//        speed -= 0.1f;

//        await UniTask.Yield();
//    }
//}

//private Vector3 Move(Vector3 position, Vector3 direction, float speed)
//{
//    return position + direction * speed * Time.deltaTime;
//}

//private void RotateBall(Vector3 direction, float speed)
//{
//    if (speed > 0)
//    {
//        // Рассчитываем угол вращения
//        float rotationAngle = speed * rotationSpeedFactor * Time.deltaTime;

//        // Определяем ось вращения (перпендикулярно направлению движения)
//        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

//        // Создаем кватернион для вращения
//        Quaternion rotation = Quaternion.LookRotation(rotationAxis, Vector3.up);

//        // Применяем вращение к текущему вращению мяча
//        _transformBody.Rotate(rotation.eulerAngles);
//    }
//}


