using BouncingBall.Game.Gameplay.LevelObject;
using BouncingBall.PrefabLoader;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root
{
    public class LevelLoader
    {
        private const string LevelsPath = "Prefabs/Gameplay/Levels/Level_";

        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly LevelFactory _levelFactory;
        [Inject] private readonly LevelManager _levelManager;

        private readonly LevelLoaderMediator _levelLoaderMediator;
        private Level _currentLevel;

        public LevelLoader(LevelLoaderMediator levelLoaderMediator)
        {
            _levelLoaderMediator = levelLoaderMediator;
            _levelLoaderMediator.CurrentLevelName.Skip(1).Subscribe(async levelName => await LoadLevelAsync(levelName));
        }

        public async UniTask LoadLevelAsync(string levelId)
        {
            if (_currentLevel != null)
            {
                GameObject.Destroy(_currentLevel.gameObject);
            }

            var levelPrefab = await LoadPrefabAsync(levelId);

            _currentLevel = _levelFactory.Create(levelPrefab);
            await _levelManager.InitLevelAsync(_currentLevel, levelId);
            _levelLoaderMediator.NotifyLevelIsLoaded();
        }

        private async UniTask<Level> LoadPrefabAsync(string levelId)
        {
            var prefabPath = LevelsPath + levelId;

            try
            {
                return await _prefabLoadStrategy.LoadPrefabAsync<Level>(prefabPath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Level with ID '{levelId}' does not exist or path to file is incorrectly specified:\n{LevelsPath}", ex);
            }
        }
    }
}
