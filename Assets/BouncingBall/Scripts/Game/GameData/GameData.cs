using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BouncingBall.Game.Data
{
    public class GameData : IDownloadable
    {
        [JsonProperty] public BallData BallData { get; private set; }
        [JsonProperty] public MushroomData MushroomData { get; private set; }
        [JsonProperty] public CactusData CactusData { get; private set; }
        [JsonProperty] public int NominalCoin { get; private set; }

        [JsonProperty] private List<string> _levelName;

        public IReadOnlyList<string> LevelName => _levelName;

        public void Load(string jsonContent)
        {
            if (jsonContent == string.Empty) return;

            var loadedData = JsonConvert.DeserializeObject<GameData>(jsonContent);
            BallData = loadedData.BallData;
            NominalCoin = loadedData.NominalCoin;
            MushroomData = loadedData.MushroomData;
            CactusData = loadedData.CactusData;
            _levelName = loadedData._levelName;
        }
    }
}
