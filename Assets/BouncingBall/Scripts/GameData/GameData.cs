using BouncingBall.Scripts.DataSystem;
using Newtonsoft.Json;

public class GameData : IDownloadable
{
    //[JsonProperty] public BallModel BallModel { get; private set; }
    //[JsonProperty] private List<LevelModel> _levelModels;

    //public IReadOnlyList<LevelModel> LevelModels => _levelModels;

    public void Load(string jsonData)
    {
        if (jsonData == string.Empty) return;
        var loadedData = JsonConvert.DeserializeObject<GameData>(jsonData);
        //BallModel = loadedData.BallModel;
        //_levelModels = loadedData._levelModels;
    }
}
