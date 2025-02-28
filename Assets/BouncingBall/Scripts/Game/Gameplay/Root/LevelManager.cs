using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Game.Gameplay.Coins;
using BouncingBall.Game.Gameplay.Entities.BallEntity;
using BouncingBall.Game.Gameplay.LevelObject;
using BouncingBall.Game.UI.GameplayState;
using BouncingBall.InputSystem.Controller;
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
        [Inject] private readonly IInputInteractivityChanger _manageInputState;

        private ReactiveCollection<Coin> _coinsCache = new();
        private CompositeDisposable _compositeDisposable;
        private LevelData _levelData;
        private GameUI _gameUI;
        private Level _level;
        private int _coinsCount;

        public LevelManager(ResetManager resetManager)
        {
            resetManager.Add(this);
        }

        public void Reset()
        {
            _compositeDisposable?.Dispose();

            foreach (var coin in _coinsCache)
            {
                coin.gameObject.SetActive(false);
            }

            _coinsCache.Clear();
            _coinsCount = 0;
            _compositeDisposable = new();
            _levelData = null;
            _level = null;
        }

        public async UniTask InitLevel(Level level, string id)
        {
            _level = level;
            _levelData = await _gameDataManager.LoadLevel(id);
            _ball.transform.position = level.BallSpawnPoint.position;
            _ball.Reset();           

            if (_attachStateUI.StateUI is GameUI gameUI)
            {
                _gameUI = gameUI;
                _gameUI.OnRestart.Subscribe(_ => RestartLevel());
            }

            if (_levelData.LevelName != MainMenuLevelName)
            {
                _manageInputState.EnableInput();

                _coinsCache.ObserveAdd().Subscribe(levelViewModel =>
                {
                    levelViewModel.Value.Reword.Subscribe(levelName => EnableLevelExit()).AddTo(_compositeDisposable);
                    levelViewModel.Value.Reword.Subscribe(count => _gameUI.AddCoin(count)).AddTo(_compositeDisposable);
                }).AddTo(_compositeDisposable);

                CreateCoins(_levelData, _level.CoinsSpawnPoint);

                _level.ExitTriggerHit.Subscribe(_ => EnableWinUI()).AddTo(_compositeDisposable);
                _gameDataManager.GameData.BallModel.ReadConcreteHealth.Subscribe(TryEnableLoseUI).AddTo(_compositeDisposable);
            }
        }

        private void CreateCoins(LevelData levelData, IReadOnlyList<Transform> spawns)
        {

            for (var i = 0; i < levelData.CoinsCount; i++)
            {
                var coins = _coinsPool.Spawn();
                coins.transform.position = spawns[i].position;
                _coinsCache.Add(coins);
            }
        }

        private async void RestartLevel()
        {
            _ball.Reset();
            _ball.transform.position = _level.BallSpawnPoint.position;
            _gameUI.DisablePopup();
            _level.Reset();
            _coinsCount = 0;

            foreach (var coin in _coinsCache)
            {
                coin.gameObject.SetActive(false);
            }

            _coinsCache.Clear();

            CreateCoins(_levelData, _level.CoinsSpawnPoint);

            _manageInputState.EnableInput();
        }

        private void EnableWinUI()
        {
            _manageInputState.DisableInput();
            _gameUI.EnableWinPopup();
        }

        private void TryEnableLoseUI(int healthCount)
        {
            if (healthCount <= 0)
            {
                _manageInputState.DisableInput();
                _gameUI.EnableLossPopup();
            }
        }

        private void EnableLevelExit()
        {
            _coinsCount++;

            if (_coinsCount >= _levelData.CoinsCount)
            {
                _level.EnableExit();
            }
        }
    }
}
