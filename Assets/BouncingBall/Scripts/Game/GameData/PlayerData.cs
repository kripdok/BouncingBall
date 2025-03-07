using BouncingBall.DataLoader;
using Newtonsoft.Json;
using UniRx;

namespace BouncingBall.Game.Data
{
    public class PlayerData : IDownloadable
    {
        [JsonProperty] private int _coinsCount;

        [JsonIgnore] public ReactiveProperty<int> CoinsCount = new();

        public void Load(string jsonContent)
        {
            if (jsonContent == string.Empty) return;

            var data = JsonConvert.DeserializeObject<PlayerData>(jsonContent);
            CoinsCount.Value = data._coinsCount;
        }
    }
}