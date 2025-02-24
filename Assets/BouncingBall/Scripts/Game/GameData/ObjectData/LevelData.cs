using BouncingBall.DataLoader;
using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public class LevelData: IDownloadable
    {
        [JsonProperty] public string LevelName { get; private set; }
        [JsonProperty] public int CoinsCount { get; private set; }
        [JsonProperty] public int CoinsForEnableExit { get; private set; }

        public void Load(string jsonData)
        {
            if (jsonData == string.Empty) return;

            var loadedData = JsonConvert.DeserializeObject<LevelData>(jsonData);
            LevelName = loadedData.LevelName;
            CoinsCount = loadedData.CoinsCount;
            CoinsForEnableExit = loadedData.CoinsForEnableExit;
        }
    }
}