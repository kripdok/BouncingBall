using BouncingBall.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [SerializeField] private Button _BackToMenuButton;

        private Action _onStartPlay;

        public void Awake()
        {
            _onStartPlay = OnExit;
            _BackToMenuButton.onClick.AddListener(_onStartPlay.Invoke);
        }

        private void OnDestroy()
        {
            _BackToMenuButton.onClick.RemoveListener(_onStartPlay.Invoke);
        }
    }
}
