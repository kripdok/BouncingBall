using BouncingBall.DataLoader;
using Newtonsoft.Json;
using UniRx;

namespace BouncingBall.Game.Data
{
    public class PlayerData : IDownloadable
    {
        [JsonProperty] private int _coinsCount;

        [JsonIgnore] public ReactiveProperty<int> CoinsCount = new();

        public void Load(string jsonData)
        {
            if (jsonData == string.Empty) return;

            var data = JsonConvert.DeserializeObject<PlayerData>(jsonData);
            CoinsCount.Value = data._coinsCount;
        }
    }
}