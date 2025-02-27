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

        private ReactiveCollection<Coin> _coinsCache = new();
        private int _coinsCount;

        private GameUI _gameUI;

        public LevelManager(ResetManager resetManager)
        {
            resetManager.Add(this);
        }

        public async UniTask InitLevel(Level level, string id)
        {
            _coinsCount = 0;
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

            if(_attachStateUI.StateUI is GameUI gameUI)
            {
                _gameUI = gameUI;
                _gameUI.OnRestart.Subscribe(_ => RestartLevel());
            }
        }

        private void CreateCoins(LevelData levelData, IReadOnlyList<Transform> spawns)
        {
            _coinsCache.ObserveAdd().Subscribe(levelViewModel =>
            {
                levelViewModel.Value.Reword.Subscribe(levelName => EnableLevelExit()).AddTo(_compositeDisposable);
                levelViewModel.Value.Reword.Subscribe(count => _gameDataManager.PlayerData.CoinsCount.Value += count).AddTo(_compositeDisposable);
            }).AddTo(_compositeDisposable);

            for (var i = 0; i < levelData.CoinsCount; i++)
            {
                var coins = _coinsPool.Spawn();
                coins.transform.position = spawns[i].position;
                _coinsCache.Add(coins);
            }

            //Настроить отслеживание подбора для UI?
        }

        private void MonitorHealthOfBall()
        {

        }

        private async void RestartLevel()
        {
            _ball.Reset();
            _ball.transform.position = _level.BallSpawnPoint.position;
            _gameUI.DisablePopup();
            _level.Reset();
            _coinsCount = 0;

            foreach (var  coin in _coinsCache)
            {
                coin.Reset();
            }

            await _gameDataManager.ResetPlayerData();
            /* Сброс мяча (его здоровье, положение, вращение и остановка ускарения)
             * Сброс UI(Ячейки здоровья, отключения всех pupop)
             * Сброс Данных игрока (Собранные монеты)
             */
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
            if (healthCount <= 0)
            {
                _gameUI.EnableLossPopup();
            }

        }

        public void Reset()
        {
            _compositeDisposable?.Dispose();

            foreach(var coin in _coinsCache)
            {
                coin.gameObject.SetActive(false);
            }

            _coinsCache.Clear();

            Debug.Log("reset");
        }
    }
}
