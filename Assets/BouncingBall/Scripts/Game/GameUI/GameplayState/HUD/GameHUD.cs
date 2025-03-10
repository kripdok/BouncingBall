using BouncingBall.Game.Data;
using BouncingBall.Game.UI.GameplayState.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState.HUD

{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private CoinCounterView _coinsCount;
        [SerializeField] private PlayerHealthView _playerHealthView;

        [Inject] private GameDataManager _gameDataManager;

        private CoinCounterViewModel _coinsCounterView;


        public void Init(Subject<string> onExit)
        {
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => onExit.OnNext("")).AddTo(this);
            InitCoinCounter();
            InitPlayerHealth();
        }

        public void AddCoin(int coin)
        {
            _coinsCounterView.AddCoin(coin);
        }

        private void InitCoinCounter()
        {
            var CoinCounterModel = new CoinCounterModel(_gameDataManager.PlayerData);
            _coinsCounterView = new CoinCounterViewModel(CoinCounterModel);
            _coinsCount.Init(_coinsCounterView);
        }

        private void InitPlayerHealth()
        {
            var playerHealthModel = new PlayerHealthModel(_gameDataManager);
            var playerHealthViewModel = new PlayerHealthViewModel(playerHealthModel);
            _playerHealthView.Init(playerHealthViewModel);
        }
    }
}
