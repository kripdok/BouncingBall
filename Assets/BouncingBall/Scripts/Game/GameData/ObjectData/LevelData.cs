using BouncingBall.DataLoader;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BouncingBall.Game.Data.ObjectData
{
    public class LevelData : IDownloadable
    {
        [JsonProperty] public string LevelName { get; private set; }
        [JsonProperty] public int CoinsCount { get; private set; }
        [JsonProperty] public int CoinsForEnableExit { get; private set; }

        [JsonProperty] public Dictionary<string, int> EnemiesCount { get; private set; }

        public void Load(string jsonContent)
        {
            if (jsonContent == string.Empty) return;

            var loadedData = JsonConvert.DeserializeObject<LevelData>(jsonContent);
            LevelName = loadedData.LevelName;
            CoinsCount = loadedData.CoinsCount;
            CoinsForEnableExit = loadedData.CoinsForEnableExit;
            EnemiesCount = loadedData.EnemiesCount;
        }
    }
}