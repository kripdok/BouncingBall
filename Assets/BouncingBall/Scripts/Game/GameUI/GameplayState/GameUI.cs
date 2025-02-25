using BouncingBall.Game.Data;
using BouncingBall.UI;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private TMP_Text _coinsCount;
        [SerializeField] private WinPopup _winPopup;
        [Header("Player health")]
        [SerializeField] private PlayerHealthCell _playerHealthCellPrefab;
        [SerializeField] private Transform _playerHeatlthContainer;

        private List<PlayerHealthCell> _playerHealthCells = new();

        [Inject] private GameDataManager _gameDataManager;

        public void Awake()
        {
            _winPopup.gameObject.SetActive(false);
            _winPopup.SetExitButton(OnExit);
            _gameDataManager.PlayerData.CoinsCount.Subscribe(count => _coinsCount.text = count.ToString()).AddTo(this);
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => OnExit.OnNext("")).AddTo(this);
            _gameDataManager.GameData.BallModel.ConcreteHealth.Skip(1).Subscribe(UpdateHealthDisplays).AddTo(this);
            CreatePlayerhealthCell();
        }

        public void EnableWinPopup()
        {
            _winPopup.gameObject.SetActive(true);
        }

        private void CreatePlayerhealthCell()
        {
            for (int i = 0; i < _gameDataManager.GameData.BallModel.MaxHealth; i++)
            {
                _playerHealthCells.Add(Instantiate(_playerHealthCellPrefab, _playerHeatlthContainer));
            }
        }

        private void UpdateHealthDisplays(int count)
        {
            var number = _gameDataManager.GameData.BallModel.MaxHealth - count;
            Debug.Log(number);

            for (int i = number-1; i >= 0; i--)
            {
                _playerHealthCells[i].DisableCell();
            }
        }
    }
}
