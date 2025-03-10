using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.Entities.BallEntity;
using BouncingBall.Game.Gameplay.Entities.EnemyEntity;
using BouncingBall.Game.Gameplay.LevelObject;
using BouncingBall.Game.UI.GameplayState;
using BouncingBall.UI.Root;
using BouncingBall.Utilities.Reset;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root
{
    public class LevelManager : IResettable
    {
        private const string MainMenuLevelName = "0";

        [Inject] private GameDataManager _gameDataManager;
        [Inject] private CoinsPool _coinsPool;
        [Inject] private Ball _ball;
        [Inject] private IAttachStateUI _attachStateUI;
        [Inject] private EnemyPool _enemyPool;

        private List<AbstractEnemy> _enemies = new();
        private ReactiveCollection<Coin> _activeCoins = new();
        private CompositeDisposable _subscriptions;
        private LevelData _levelData;
        private GameUI _gameUI;
        private Level _level;
        private int _coinsCollected;

        public LevelManager(ResetManager resetManager)
        {
            resetManager.RegisterResettable(this);
        }

        public void Reset()
        {
            _subscriptions?.Dispose();

            foreach (var coin in _activeCoins)
            {
                coin.gameObject.SetActive(false);
            }

            foreach (var enemy in _enemies)
            {
                _enemyPool.Remove(enemy);
            }

            _activeCoins.Clear();
            _coinsCollected = 0;
            _subscriptions = new CompositeDisposable();
            _levelData = null;
            _level = null;
        }

        public async UniTask InitLevelAsync(Level level, string levelId)
        {
            _level = level;
            _levelData = await _gameDataManager.LoadLevelDataAsync(levelId);
            _ball.transform.position = level.BallSpawnPoint.position;
            _ball.Reset();

            if (_attachStateUI.StateUI is GameUI gameUI)
            {
                _gameUI = gameUI;
                _gameUI.OnRestart.Subscribe(_ => RestartLevel()).AddTo(_subscriptions);
            }

            if (_levelData.LevelName != MainMenuLevelName)
            {
                _activeCoins.ObserveAdd().Subscribe(coinEvent =>
                {
                    coinEvent.Value.Reword.Subscribe(_ => CheckAndEnableLevelExit()).AddTo(_subscriptions);
                    coinEvent.Value.Reword.Subscribe(count => _gameUI.AddCoin(count)).AddTo(_subscriptions);
                }).AddTo(_subscriptions);

                SpawnCoins(_levelData, _level.CoinsSpawnPoint);
                SpawnEnemies(_levelData, _level.EnemySpawnPoint);

                _level.ExitTriggerHit.Subscribe(_ => ShowWinPopup()).AddTo(_subscriptions);
                _gameDataManager.GameData.BallData.HealthSystem.CurrentHealth.Subscribe(CheckAndShowLosePopup).AddTo(_subscriptions);
            }
        }

        private void SpawnCoins(LevelData levelData, IReadOnlyList<Transform> spawnPoints)
        {
            for (var i = 0; i < levelData.CoinsCount; i++)
            {
                var coin = _coinsPool.Spawn();
                coin.transform.position = spawnPoints[i].position;
                _activeCoins.Add(coin);
            }
        }

        private void SpawnEnemies(LevelData levelData, IReadOnlyList<Transform> spawnPoints)
        {
            var spawnIndex = 0;

            foreach (var enemyType in levelData.EnemiesCount.Keys)
            {
                for (int i = 0; i < levelData.EnemiesCount[enemyType]; i++)
                {
                    var enemy = _enemyPool.Create(spawnPoints[spawnIndex].position, enemyType);
                    _enemies.Add(enemy);
                    spawnIndex++;
                }
            }
        }

        private async void RestartLevel()
        {
            _ball.Reset();
            _ball.transform.position = _level.BallSpawnPoint.position;
            _gameUI.DisablePopup();
            _level.Reset();
            _coinsCollected = 0;

            foreach (var coin in _activeCoins)
            {
                coin.gameObject.SetActive(false);
            }

            _activeCoins.Clear();

            foreach (var enemy in _enemies)
            {
                _enemyPool.Remove(enemy);
            }

            _enemies.Clear();

            SpawnCoins(_levelData, _level.CoinsSpawnPoint);
            SpawnEnemies(_levelData, _level.EnemySpawnPoint);
        }

        private void ShowWinPopup()
        {
            _gameUI.EnableWinPopup();
        }

        private void CheckAndShowLosePopup(int healthCount)
        {
            if (healthCount <= 0)
            {
                _gameUI.EnableLossPopup();
            }
        }

        private void CheckAndEnableLevelExit()
        {
            _coinsCollected++;

            if (_coinsCollected >= _levelData.CoinsForEnableExit)
            {
                _level.EnableExit();
            }
        }
    }
}
