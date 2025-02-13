using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public class PlayerInputSimulator : IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionSet { get; private set; }
        public ReactiveProperty<Vector3> Direction { get; private set; }
        public ReactiveProperty<float> ZScale { get; private set; }

        private float _changeSpeed = 1.0f; // Скорость изменения значений
        private float _cooldownTime; // Время кулдауна
        private float _minCooldown = 1.0f; // Минимальное время кулдауна
        private float _maxCooldown = 3.0f; // Максимальное время кулдауна

        public PlayerInputSimulator()
        {
            IsDirectionSet = new ReactiveProperty<bool>(false);
            Direction = new ReactiveProperty<Vector3>(Vector3.zero);
            ZScale = new ReactiveProperty<float>(0f);
        }


        private float elapsedTime;
        private Vector3 randomDirection;
        private float randomZScale;
        private Vector3 initialDirection;
        private float initialZScale;

        public void SetRotationAndScale()
        {
            if (!IsDirectionSet.Value)
            {
                randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                randomZScale = Random.Range(0.5f, 3f); // Генерация случайного значения ZScale
                IsDirectionSet.Value = true;

                elapsedTime = 0f;
                initialDirection = Direction.Value;
                initialZScale = ZScale.Value;
            }
            else
            {
                if (elapsedTime < 1f)
                {
                    elapsedTime += Time.deltaTime * _changeSpeed;
                    Direction.Value = Vector3.Lerp(initialDirection, randomDirection, elapsedTime); // Изменяем направление
                    ZScale.Value = Mathf.Lerp(initialZScale, randomZScale, elapsedTime);
                    IsDirectionSet.Value = false;
                }
            }
        }

        public void TryDisableIsDirectionSet()
        {
        }
    }
}
