using BouncingBall.Game.UI.GameplayState.HUD;
using BouncingBall.Game.UI.GameplayState.Screen;
using BouncingBall.UI;
using BouncingBall.Utilities;
using UniRx;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [Inject] private WinScreen _winPopup;
        [Inject] private LossScreen _lossPopup;
        [Inject] private GameHUD _hud;
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

            _winPopup.SetExitButton(ExitRequested);
            _lossPopup.SetExitButton(ExitRequested);
            _lossPopup.SetRestartButton(RestartRequested);
        }

        private void InitializeHUD()
        {
            _hud.Init(ExitRequested);
        }
    }
}
