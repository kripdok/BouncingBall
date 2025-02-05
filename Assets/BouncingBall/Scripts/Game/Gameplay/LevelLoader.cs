using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class LevelLoader
    {
        private const string LevelsPathc = "Prefabs/Gameplay/Levels/Level_";

        private readonly IPrefabLoadStrategy _prefabLoadStrategy;
        private readonly LevelFactory _levelFactory;


        private Level _concreteLevel;
        private string _nameLoadingLevel;

        public LevelLoader(LevelFactory levelFactory, IPrefabLoadStrategy prefabLoadStrategy)
        {
            _prefabLoadStrategy = prefabLoadStrategy;
            _levelFactory = levelFactory;
        }

        public void SetLevelForLoading(string id)
        {
            _nameLoadingLevel = id;
        }

        public async UniTask LoadLevel(string id)
        {
            if (_concreteLevel != null)
            {
                GameObject.Destroy(_concreteLevel.gameObject);
            }

            var patch = LevelsPathc + id;
            var prefab = await _prefabLoadStrategy.AsyncLoadPrefab<Level>(patch);

            if (prefab == null)
            {
                throw new ArgumentNullException($"Level with ID {id} ​​does not exist or path to file is incorrectly specified:\n{LevelsPathc}");
            }

            _concreteLevel = _levelFactory.Create(prefab);
        }
    }
}
