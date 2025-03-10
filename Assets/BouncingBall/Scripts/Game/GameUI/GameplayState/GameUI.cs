using BouncingBall.Game.UI.GameplayState.HUD;
using BouncingBall.UI;
using BouncingBall.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LossPopup _lossPopup;
        [SerializeField] private GameHUD _hud;

        [Inject] private IPausable _pausable;

        public Subject<Unit> RestartRequested = new();

        public void Awake()
        {
            InitializePopups();
            InitializeHUD();
        }

        public void ShowWinPopup()
        {
            _pausable.Pause();
            _winPopup.gameObject.SetActive(true);
        }

        public void ShowLossPopup()
        {
            _pausable.Pause();
            _lossPopup.gameObject.SetActive(true);
        }

        public void HidePopups()
        {
            _pausable.Resume();
            _winPopup.gameObject.SetActive(false);
            _lossPopup.gameObject.SetActive(false);
        }

        public void AddCoin(int coin)
        {
            _hud.AddCoin(coin);
        }

        private void InitializePopups()
        {
            HidePopups();

            _winPopup.SetExitButton(OnExit);
            _lossPopup.SetExitButton(OnExit);
            _lossPopup.SetRestartButton(RestartRequested);
        }

        private void InitializeHUD()
        {
            _hud.Init(OnExit);
        }
    }
}
