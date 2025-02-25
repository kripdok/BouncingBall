using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Game.Gameplay.BallObject;
using BouncingBall.Game.Gameplay.Coins;
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

        [Inject] private GameDataManager _gameDataManager;
        [Inject] private CoinsPool _coinsPool;
        [Inject] private Ball _ball;
        [Inject] private IAttachStateUI _attachStateUI;

        private Level _level;
        private LevelData _levelData;

        private CompositeDisposable _compositeDisposable;

        private ReactiveDictionary<CoinData, Coin> _coinsCache = new();
        private int _coinsCount;

        private GameUI _gameUI;

        public async UniTask InitLevel(Level level, string id)
        {
            _compositeDisposable = new();
            _levelData = null; //Добавить в ресет
            _level = level;
            _levelData = await _gameDataManager.LoadLevel(id);



            _ball.transform.position = level.BallSpawnPoint.position;

            if (_levelData.LevelName != "0")
            {
                CreateCoins(_levelData, level.CoinsSpawnPoint);

                _level.ExitTriggerHit.Subscribe(_ => EnableWinUI()).AddTo(_compositeDisposable);
                _gameDataManager.GameData.BallModel.ReadConcreteHealth.Subscribe(TryEnableLoseUI).AddTo(_compositeDisposable);
            }

            if(_attachStateUI.StateUI as GameUI)
            {
                _gameUI = _attachStateUI.StateUI.GetComponent<GameUI>();
                
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
                var coinData = new CoinData(_gameDataManager.GameData.NominalCoiny);
                var coins = _coinsPool.Spawn(coinData);
                coins.transform.position = spawns[i].position;
                _coinsCache.Add(coinData, coins);
                coinData.Reword.Subscribe(count => _gameDataManager.PlayerData.CoinsCount.Value += count).AddTo(_compositeDisposable);
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
            _gameUI.EnableWinPopup();
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
