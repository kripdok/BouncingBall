using BouncingBall.DataLoader;
using Newtonsoft.Json;

namespace BouncingBall.Game.Data.ObjectData
{
    public class AdsData : IDownloadable
    {
        [JsonProperty] public string GameAndroidId { get;private set;}
        [JsonProperty] public string GameIOSId { get;private set;}
        [JsonProperty] public string InterstitialBannerAndroidId { get;private set;}
        [JsonProperty] public string InterstitialBannerIOSId { get;private set;}

        public void Load(string jsonContent)
        {
            if (jsonContent == string.Empty) return;

            var data = JsonConvert.DeserializeObject<AdsData>(jsonContent);
            GameAndroidId = data.GameAndroidId;
            GameIOSId = data.GameIOSId;
            InterstitialBannerAndroidId = data.InterstitialBannerAndroidId;
            InterstitialBannerIOSId = data.InterstitialBannerIOSId;
        }
    }
}
