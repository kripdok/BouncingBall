using UniRx;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.BallSystem
{
    public class BallModel
    {
        [JsonProperty] private int _maxHealth;

        public int MaxHealth => _maxHealth;

        public ReadOnlyReactiveProperty<int> ReadConcreteHealth;
        public ReadOnlyReactiveProperty<Vector3> ReadPosition;
        public ReadOnlyReactiveProperty<Vector3> ReadDirection;


        public ReactiveProperty<int> ConcreteHealth;
        public ReactiveProperty<Vector3> Position = new();
        public ReactiveProperty<Vector3> Direction = new();

        public BallModel()
        {
            _maxHealth = 3;
            ConcreteHealth = new ReactiveProperty<int>(MaxHealth);
            ReadConcreteHealth = new(ConcreteHealth);
            ReadPosition = new(Position);
            ReadDirection = new(Direction);
        }

        public void AddDamage(int damage)
        {
            ConcreteHealth.Value -= damage;
        }

        public void RestoreHealth()
        {
            ConcreteHealth.Value = _maxHealth;
        }
    }
}
