using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.Entities.BallEntity;
using BouncingBall.Game.Gameplay.Entities.EnemyEntity;
using BouncingBall.Game.Gameplay.LevelObject;
using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Game.UI.GameplayState;
using BouncingBall.UI.Root;
using BouncingBall.Utilities.Reset;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Root
{
    public class LevelManager : IResettable
    {
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
        private int _coinsRequiredToExit;

        public LevelManager(ResetManager resetManager)
        {
            resetManager.RegisterResettable(this);
        }

        public void Reset()
        {
            _subscriptions?.Dispose();

            ClearActiveEntities();

            _subscriptions = new CompositeDisposable();
            _levelData = null;
            _level = null;
        }

        public async UniTask InitLevelAsync(Level level, string levelId)
        {
            _level = level;
            _levelData = await _gameDataManager.LoadLevelDataAsync(levelId);

            InitializeBall();
            SubscribeToUIEvents();

            if (_levelData.LevelName != MainMenuTag.Name)
            {
                SubscribeToCoinEvents();
                SpawnLevelEntities();
                SubscribeToLevelEvents();
            }
        }

        private void ClearActiveEntities()
        {
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
            _enemies.Clear();
        }

        private void InitializeBall()
        {
            _ball.transform.position = _level.BallSpawnPoint.position;
            _ball.Reset();
        }

        private void SubscribeToUIEvents()
        {
            if (_attachStateUI.StateUI is GameUI gameUI)
            {
                _gameUI = gameUI;
                _gameUI.RestartRequested.Subscribe(_ => RestartLevel()).AddTo(_subscriptions);
            }
        }

        private void SubscribeToCoinEvents()
        {
            _activeCoins.ObserveAdd().Subscribe(coinEvent =>
            {
                coinEvent.Value.OnRewardCollected.Subscribe(_ => UpdateExitCondition()).AddTo(_subscriptions);
                coinEvent.Value.OnRewardCollected.Subscribe(count => _gameUI.AddCoin(count)).AddTo(_subscriptions);
            }).AddTo(_subscriptions);
        }

        private void SpawnLevelEntities()
        {
            SpawnCoins(_levelData, _level.CoinsSpawnPoint);
            SpawnEnemies(_levelData, _level.EnemySpawnPoint);
        }

        private void SubscribeToLevelEvents()
        {
            _level.OnExitTriggerHit.Subscribe(_ => HandleWinCondition()).AddTo(_subscriptions);
            _gameDataManager.GameData.BallData.HealthSystem.CurrentHealth.Subscribe(HandleHealthChange).AddTo(_subscriptions);
        }

        private void SpawnCoins(LevelData levelData, IReadOnlyList<Transform> spawnPoints)
        {
            var coinsCount = levelData.CoinsCount;

            if (levelData.CoinsCount > spawnPoints.Count)
            {
                coinsCount = spawnPoints.Count;
                Debug.LogWarning($"In the level data, the number of coins ({levelData.CoinsCount}) exceeds the number of positions in the prefab ({spawnPoints.Count}). " +
                    $"Coins will be created according to the number of spawn positions.");
            }

            for (var i = 0; i < coinsCount; i++)
            {
                var coin = _coinsPool.Spawn();
                coin.transform.position = spawnPoints[i].position;
                _activeCoins.Add(coin);
            }

            SetCoinsRequiredToExit();
        }

        private void SetCoinsRequiredToExit()
        {
            _coinsRequiredToExit = _levelData.CoinsForEnableExit > _activeCoins.Count ? _activeCoins.Count : _levelData.CoinsForEnableExit;

            if (_levelData.CoinsForEnableExit > _activeCoins.Count)
            {
                Debug.LogWarning($"The number of coins required to enable the exit ({_levelData.CoinsForEnableExit}) exceeds the number of coins created ({_activeCoins.Count}). " +
                    $"The number of coins is set to the number of coins created.");
            }
        }

        private void SpawnEnemies(LevelData levelData, IReadOnlyList<Transform> spawnPoints)
        {
            int totalEnemies = levelData.EnemiesCount.Values.Sum();

            if (totalEnemies > spawnPoints.Count)
            {
                Debug.LogWarning($"The number of monsters ({totalEnemies}) exceeds the number of spawn points ({spawnPoints.Count})." +
                    $" Only {spawnPoints.Count} monsters will be spawned.");
                totalEnemies = spawnPoints.Count;
            }

            var spawnIndex = 0;

            foreach (var enemyType in levelData.EnemiesCount.Keys)
            {
                for (int i = 0; i < levelData.EnemiesCount[enemyType]; i++)
                {
                    if (spawnIndex >= totalEnemies)
                        break;

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
            _gameUI.HidePopups();
            _level.Reset();

            ClearActiveEntities();

            SpawnCoins(_levelData, _level.CoinsSpawnPoint);
            SpawnEnemies(_levelData, _level.EnemySpawnPoint);
        }

        private void HandleWinCondition()
        {
            _gameUI.ShowWinPopup();
        }

        private void HandleHealthChange(int healthCount)
        {
            if (healthCount <= 0)
            {
                _gameUI.ShowLossPopup();
            }
        }

        private void UpdateExitCondition()
        {
            _coinsCollected++;

            if (_coinsCollected >= _coinsRequiredToExit)
            {
                _level.EnableExit();
            }
        }
    }
}
