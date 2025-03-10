using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;

namespace BouncingBall.Game.Data
{
    public class GameDataManager
    {
        private const string GameDataFilePath = "Assets/BouncingBall/Resources/Data/Data.json";
        private const string PlayerDataFilePath = "Assets/BouncingBall/Resources/Data/PlayerData.json";
        private const string LevelDataFilePathTemplate = "Assets/BouncingBall/Resources/Data/Levels/Level_NAME.json";

        public GameData GameData { get; private set; }
        public PlayerData PlayerData { get; private set; }

        [Inject] private IDataLoader _dataLoader;

        private readonly Dictionary<string, LevelData> _cachedLevelData = new();

        public async UniTask LoadGameDataAsync()
        {
            GameData = await _dataLoader.LoadDataFromPathAsync<GameData>(GameDataFilePath);
            PlayerData = await _dataLoader.LoadDataFromPathAsync<PlayerData>(PlayerDataFilePath);
        }

        public async UniTask<LevelData> LoadLevelDataAsync(string levelName)
        {
            if (_cachedLevelData.TryGetValue(levelName, out var levelData))
            {
                return levelData;
            }

            string levelDataFilePath = LevelDataFilePathTemplate.Replace("NAME", levelName);
            levelData = await _dataLoader.LoadDataFromPathAsync<LevelData>(levelDataFilePath);
            _cachedLevelData[levelName] = levelData;

            return levelData;
        }

        public async UniTask ResetPlayerDataAsync()
        {
            var playerData = await _dataLoader.LoadDataFromPathAsync<PlayerData>(PlayerDataFilePath);
            PlayerData.CoinsCount.Value = playerData.CoinsCount.Value;
        }
    }
}