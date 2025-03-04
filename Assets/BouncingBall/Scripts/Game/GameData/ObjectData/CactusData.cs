using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public class CactusData : EnemyData
    {
        [JsonProperty] public float MaxMoveSpeed { get; private set; }
    }
}
