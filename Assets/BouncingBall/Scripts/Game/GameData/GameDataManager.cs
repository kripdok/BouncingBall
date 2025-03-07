using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

namespace BouncingBall.Game.Data
{
    public class GameDataManager
    {
        private const string GameDataPath = "Assets/Resources/Data.json";
        private const string PlayerDataPath = "Assets/Resources/PlayerData.json";
        private const string LevelDataPath = "Assets/Resources/Levels/Level_NAME.json";

        public GameData GameData { get; private set; }
        public PlayerData PlayerData { get; private set; }

        [Inject] private IDataLoader _dataLoader;

        private Dictionary<string, LevelData> _levelData = new();

        public async UniTask LoadGameData()
        {
            GameData = await _dataLoader.LoadDataFromPathAsync<GameData>(GameDataPath);
            PlayerData = await _dataLoader.LoadDataFromPathAsync<PlayerData>(PlayerDataPath);
        }

        public async UniTask<LevelData> LoadLevel(string name)
        {
            if (_levelData.TryGetValue(name, out var level))
                return level;

            level = await _dataLoader.LoadDataFromPathAsync<LevelData>(LevelDataPath.Replace("NAME", name));
            _levelData[name] = level;
            return level;
        }

        public async UniTask ResetPlayerData()
        {
            var playerData = await _dataLoader.LoadDataFromPathAsync<PlayerData>(PlayerDataPath);
            PlayerData.CoinsCount.Value = playerData.CoinsCount.Value;
        }
    }
}