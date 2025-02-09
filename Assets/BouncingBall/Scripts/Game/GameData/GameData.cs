using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BouncingBall.Game.Data
{
    public class GameData : IDownloadable
    {
        [JsonProperty] public BallData BallModel { get; private set; }
        [JsonProperty] private List<LevelData> _levelData;

        public IReadOnlyList<LevelData> LevelData => _levelData;

        public void Load(string jsonData)
        {
            if (jsonData == string.Empty) return;

            var loadedData = JsonConvert.DeserializeObject<GameData>(jsonData);
            BallModel = loadedData.BallModel;
            _levelData = loadedData._levelData;
        }
    }
}
