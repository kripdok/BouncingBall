using BouncingBall.Scripts.DataSystem;
using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GameData : IDownloadable
{
    [JsonProperty] public BallModel BallModel { get; private set; }
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
