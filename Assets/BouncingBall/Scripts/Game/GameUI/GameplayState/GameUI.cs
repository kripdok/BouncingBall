using Assets.BouncingBall.Scripts.Game.GameUI;
using Assets.BouncingBall.Scripts.Game.GameUI.AA;
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

        public Subject<Unit> OnRestart = new();

        public void Awake()
        {
            InitPopup();
            InitHUD();
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
            _hud.AddCoin(coin);
        }

        private void InitPopup()
        {
            DisablePopup();

            _winPopup.SetExitButton(OnExit);
            _lossPopup.SetExitButton(OnExit);
            _lossPopup.SetRestartButton(OnRestart);
        }

        private void InitHUD()
        {
            _hud.Init(OnExit);
        }
    }
}
