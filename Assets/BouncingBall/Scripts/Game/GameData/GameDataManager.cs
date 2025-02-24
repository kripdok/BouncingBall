using BouncingBall.DataLoader;
using BouncingBall.Game.Data.ObjectData;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace BouncingBall.Game.Data
{
    public class GameDataManager
    {
        public GameData GameData { get; private set; }

        private IDataLoader _dataLoader;

        private Dictionary<string, LevelData> _levelData;

        public GameDataManager(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public async UniTask LoadGameData()
        {
            GameData = await _dataLoader.LoadDataAsync<GameData>("Assets/Resources/Data.json");
        }

        public async UniTask<LevelData> LoadLevel(string name)
        {
            if(_levelData.ContainsKey(name)) 
                return _levelData[name];

            var level = await _dataLoader.LoadDataAsync<LevelData>($"Assets/Resources/Levels/Level_{name}.json");
            _levelData[name] = level;
            return level;
        }
    }
}