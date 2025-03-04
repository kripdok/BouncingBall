using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public abstract class EnemyData
    {
        [JsonProperty] public int MaxHealthAmount { get; private set; }
    }
}
