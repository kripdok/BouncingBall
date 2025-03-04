using BouncingBall.Game.Data;
using BouncingBall.Game.UI.GameplayState.MVVM;
using BouncingBall.UI;
using BouncingBall.Utilities;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private CoinCounterViewModel _coinsCount;
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LossPopup _lossPopup;
        [Header("Player health")]
        [SerializeField] private PlayerHealthCell _playerHealthCellPrefab;
        [SerializeField] private Transform _playerHeatlthContainer;

        [Inject] private GameDataManager _gameDataManager;
        [Inject] private IPausable _pausable;

        private List<PlayerHealthCell> _playerHealthCells = new();
        private CoinCounterView _coinsCounterView;

        public Subject<Unit> OnRestart = new();

        public void Awake()
        {
            InitPopup();
            Subsctibe();
            InitCoinCounter();
            CreatePlayerhealthCell();
        }

        public void EnableWinPopup()
        {
            _pausable.Pause();
            _winPopup.gameObject.SetActive(true);
        }

        public void EnableLossPopup()
        {
            _pausable.Pause();
            _lossPopup.gameObject.SetActive(true);
        }

        public void DisablePopup()
        {
            _pausable.Resume();
            _winPopup.gameObject.SetActive(false);
            _lossPopup.gameObject.SetActive(false);
        }

        public void AddCoin(int coin)
        {
            _coinsCounterView.AddCoin(coin);
        }

        private void Subsctibe()
        {
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => OnExit.OnNext("")).AddTo(this);
            _gameDataManager.GameData.BallData.HealthSystem.CorrectAmount.Skip(1).Subscribe(UpdateHealthDisplays).AddTo(this);
        }

        private void InitCoinCounter()
        {
            var CoinCounterModel = new CoinCounterModel(_gameDataManager.PlayerData);
            _coinsCounterView = new CoinCounterView(CoinCounterModel);
            _coinsCount.Init(_coinsCounterView);
        }

        private void InitPopup()
        {
            DisablePopup();

            _winPopup.SetExitButton(OnExit);
            _lossPopup.SetExitButton(OnExit);
            _lossPopup.SetRestartButton(OnRestart);
        }

        private void CreatePlayerhealthCell()
        {
            for (int i = 0; i < _gameDataManager.GameData.BallData.HealthSystem.MaxAmount; i++)
            {
                _playerHealthCells.Add(Instantiate(_playerHealthCellPrefab, _playerHeatlthContainer));
            }
        }

        private void UpdateHealthDisplays(int currentHealth)
        {
            int maxHealth = _gameDataManager.GameData.BallData.HealthSystem.MaxAmount;

            for (int i = 0; i < maxHealth; i++)
            {
                if (i < currentHealth)
                {
                    _playerHealthCells[i].EnableCell();
                }
                else
                {
                    _playerHealthCells[i].DisableCell();
                }
            }
        }
    }
}
