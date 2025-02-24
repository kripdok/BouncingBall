using BouncingBall.Game.Data;
using BouncingBall.UI;
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

        [Inject] private GameDataManager _gameDataManager;

        public void Awake()
        {
            _gameDataManager.PlayerData.CoinsCount.Subscribe(count=>_coinsCount.text = count.ToString()).AddTo(this);
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => OnExit.OnNext("")).AddTo(this);
        }
    }
}
