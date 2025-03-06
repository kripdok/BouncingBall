﻿using Newtonsoft.Json;
using System;
using UniRx;

namespace BouncingBall.Utilities.HealthSystems
{
    public class HealthSystem
    {
        [JsonProperty] public int MaxAmount { get; private set; }

        public ReactiveProperty<int> CorrectAmount { get; private set; }

        public HealthSystem(int maxAmount)
        {
            MaxAmount = maxAmount;
            CorrectAmount = new();
            Reset();
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Object can't take negative damage! Damage: {amount}");
            }

            CorrectAmount.Value -= amount;
        }

        public void Reset()
        {
            CorrectAmount.Value = MaxAmount;
        }
    }
}
