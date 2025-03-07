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
        private const string LevelsPathc = "Prefabs/Gameplay/Levels/Level_";

        [Inject] private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        [Inject] private readonly LevelFactory _levelFactory;
        [Inject] private readonly LevelManager _manager;

        private readonly LevelLoaderMediator _levelLoaderMediator;

        private Level _concreteLevel;

        public LevelLoader(LevelLoaderMediator levelLoaderMediator)
        {
            _levelLoaderMediator = levelLoaderMediator;
            _levelLoaderMediator.CurrentLevelName.Skip(1).Subscribe(async levelName => await LoadLevel(levelName));
        }

        public async UniTask LoadLevel(string id)
        {
            if (_concreteLevel != null)
            {
                GameObject.Destroy(_concreteLevel.gameObject);
            }

            var prefab = await LoadPrefab(id);

            _concreteLevel = _levelFactory.Create(prefab);
            await _manager.InitLevel(_concreteLevel, id);
            _levelLoaderMediator.NotifyLevelIsLoaded();
        }

        private async UniTask<Level> LoadPrefab(string id)
        {
            var patch = LevelsPathc + id;

            try
            {
                return await _prefabLoadStrategy.LoadPrefabAsync<Level>(patch);
            }
            catch
            {
                throw new ArgumentNullException($"Level with ID {id} ​​does not exist or path to file is incorrectly specified:\n{LevelsPathc}");
            }
        }
    }
}
