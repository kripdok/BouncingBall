using Newtonsoft.Json;

namespace BouncingBall.Scripts.Game.Gameplay.LevelSystem
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
