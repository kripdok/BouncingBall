using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplayManager
    {
        private LevelFactory _levelFactory;
        private Level _level;

        public GameplayManager(LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
            var levelModel = _levelFactory.Create();
            var levelPrefab = Resources.Load<Level>("Prefabs/Gameplay/Levels/Level_1");
            _level = GameObject.Instantiate(levelPrefab);
            _level.Init(levelModel);
        }
    }
}
