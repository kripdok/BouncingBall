using BouncingBall.Game.Data.ObjectData;
using Newtonsoft.Json;

namespace BouncingBall.Game.UI.MVVM.Level
{
    public class LevelModel
    {
        [JsonProperty] public readonly string LevelName;

        public LevelModel(LevelData levelData)
        {
            LevelName = levelData.LevelName;
        }
    }
}
