using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BouncingBall.Game.Data
{
    public class GameData : IDownloadable
    {
        [JsonProperty] public BallData BallModel { get; private set; }
        [JsonProperty] public int NominalCoiny { get; private set; }
        [JsonProperty] private List<string> _levelName;

        public IReadOnlyList<string> LevelName => _levelName;

        public void Load(string jsonData)
        {
            if (jsonData == string.Empty) return;

            var loadedData = JsonConvert.DeserializeObject<GameData>(jsonData);
            BallModel = loadedData.BallModel;
            NominalCoiny = loadedData.NominalCoiny;
            _levelName = loadedData._levelName;
        }
    }
}
