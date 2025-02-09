using Newtonsoft.Json;

namespace BouncingBall.Scripts.Game.Gameplay.LevelSystem
{
    public class LevelModel
    {
        [JsonProperty]public readonly string LevelName;

        public LevelModel(string levelName)
        {
            LevelName = levelName;
        }
    }
}
