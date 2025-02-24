using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public class LevelData
    {
        [JsonProperty] public readonly string LevelName;

        [JsonProperty] public readonly int CoinsCount;
        [JsonProperty] public readonly int CoinsForEnableExit;

        public LevelData(string levelName)
        {
            LevelName = levelName;
            CoinsCount = 0;
            CoinsForEnableExit = 0;
        }
    }
}
