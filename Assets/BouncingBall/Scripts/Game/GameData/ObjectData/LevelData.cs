using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public class LevelData
    {
        [JsonProperty] public readonly string LevelName;

        public LevelData(string levelName)
        {
            LevelName = levelName;
        }
    }
}
