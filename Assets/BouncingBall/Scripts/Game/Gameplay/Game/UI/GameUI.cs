using System;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Scripts.Game.Gameplay.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button _BackToMenuButton;

        private Action _onStartPlay;

        public void Init(Action OnBackToMenu)
        {
            _onStartPlay = OnBackToMenu;
            _BackToMenuButton.onClick.AddListener(_onStartPlay.Invoke);
        }

        private void OnDestroy()
        {
            _BackToMenuButton.onClick.RemoveListener(_onStartPlay.Invoke);
        }
    }
}
