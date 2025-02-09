using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.BallSystem
{
    public class BallModel
    {
        [JsonProperty]private int _maxHealth;

        [JsonIgnore]public int MaxHealth => _maxHealth;

        [JsonIgnore] public ReadOnlyReactiveProperty<int> ReadConcreteHealth;
        [JsonIgnore] public ReadOnlyReactiveProperty<Vector3> ReadPosition;
        [JsonIgnore] public ReadOnlyReactiveProperty<Vector3> ReadDirection;


        [JsonIgnore] public ReactiveProperty<int> ConcreteHealth;
        [JsonIgnore] public ReactiveProperty<Vector3> Position = new();
        [JsonIgnore] public ReactiveProperty<Vector3> Direction = new();

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
