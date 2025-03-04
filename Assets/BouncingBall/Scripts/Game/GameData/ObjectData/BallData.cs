using BouncingBall.Utilities.HealthSystems;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.Data.ObjectData
{
    public class BallData
    {
        [JsonProperty] public HealthSystem HealthSystem { get; private set; }
        [JsonProperty] public int MaxSpeed { get; private set; }

        [JsonIgnore] public ReactiveProperty<Vector3> Position = new();
        [JsonIgnore] public ReactiveProperty<Vector3> Direction = new();
    }
}
