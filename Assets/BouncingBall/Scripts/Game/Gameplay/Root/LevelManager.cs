using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Game.Gameplay.BallObject;
using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.LevelObject;
using BouncingBall.Game.UI.MVVM.Level;
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

        private Level _level;
        private LevelData _levelData;

        private CompositeDisposable _compositeDisposable;

        private ReactiveDictionary<CoinData, Coin> _coinsCache = new();
        private int _coinsCount;
        
        public void InitLevel(Level level, string id)
        {
            _compositeDisposable = new();
            _levelData = null; //Добавить в ресет
            _level = level;
            _levelData = _gameDataManager.GameData.LevelData.FirstOrDefault(level => level.LevelName == id);



            _ball.transform.position = level.BallSpawnPoint.position;

            if (_levelData != null)
            {
                CreateCoins(_levelData, level.CoinsSpawnPoint);

                _level.ExitTriggerHit.Subscribe(_ => EnableWinUI()).AddTo(_compositeDisposable);
                _gameDataManager.GameData.BallModel.ReadConcreteHealth.Subscribe(TryEnableLoseUI).AddTo(_compositeDisposable);
            }
        }

        private void CreateCoins(LevelData levelData, IReadOnlyList<Transform> spawns)
        {
            _coinsCount = 0;
            _coinsCache.ObserveAdd().Subscribe(levelViewModel =>
            {
                levelViewModel.Key.Reword.Subscribe(levelName => EnableLevelExit()).AddTo(_compositeDisposable);
            }).AddTo(_compositeDisposable);

            for (var i = 0; i < levelData.CoinsCount; i++)
            {
                var coinData = new CoinData(10);
                var coins = _coinsPool.Spawn(coinData);
                coins.transform.position = spawns[i].position;
                _coinsCache.Add(coinData,coins);
            }

            //Настроить отслеживание подбора для UI?
        }

        private void MonitorHealthOfBall()
        {

        }

        private void RestartLevel()
        {
            _ball.Reset();
            _ball.transform.position = _level.BallSpawnPoint.position;
            //
        }

        private void EnableLevelExit()
        {
            _coinsCount++;
            if (_coinsCount >= _levelData.CoinsCount) 
            //TODO - сделать проверку на собраныне монеты и на количество необходимое для открытия уровыня
            _level.EnableExit();
        }

        private void EnableWinUI()
        {
            //Нужен UI
            //
        }

        private void TryEnableLoseUI(int healthCount)
        {
            if (healthCount < 0)
            {

            }
            //Нужен UI
        }

        public void Reset()
        {
            _compositeDisposable.Dispose();
        }
    }
}
