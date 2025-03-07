using Newtonsoft.Json;
using System;
using UniRx;

namespace BouncingBall.Utilities.HealthSystems
{
    public class HealthSystem
    {
        [JsonProperty] public int MaxHealth { get; private set; }

        public ReactiveProperty<int> CurrentHealth { get; private set; }

        public HealthSystem(int maxAmount)
        {
            MaxHealth = maxAmount;
            CurrentHealth = new();
            Reset();
        }

        public void ApplyDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Object can't take negative damage! Damage: {amount}");
            }

            CurrentHealth.Value -= amount;
        }

        public void Reset()
        {
            CurrentHealth.Value = MaxHealth;
        }
    }
}
