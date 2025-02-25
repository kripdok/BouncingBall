using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.Data.ObjectData
{
    public class BallData
    {
        [JsonProperty] public int MaxHealth { get; private set; }
        [JsonIgnore] public int MaxSpeed = 5;


        [JsonIgnore] public ReadOnlyReactiveProperty<int> ReadConcreteHealth;
        [JsonIgnore] public ReadOnlyReactiveProperty<Vector3> ReadPosition;
        [JsonIgnore] public ReadOnlyReactiveProperty<Vector3> ReadDirection;


        [JsonIgnore] public ReactiveProperty<int> ConcreteHealth;
        [JsonIgnore] public ReactiveProperty<Vector3> Position = new();
        [JsonIgnore] public ReactiveProperty<Vector3> Direction = new();

        public BallData()
        {
            ConcreteHealth = new ReactiveProperty<int>(MaxHealth);
            ReadConcreteHealth = new(ConcreteHealth);
            ReadPosition = new(Position);
            ReadDirection = new(Direction);
            ResetHealth();
        }

        public void AddDamage(int damage)
        {
            ConcreteHealth.Value -= damage;
        }

        public void ResetHealth()
        {
            ConcreteHealth.Value = MaxHealth;
        }
    }
}
