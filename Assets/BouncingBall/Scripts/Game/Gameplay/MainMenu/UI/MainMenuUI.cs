using System;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Scripts.Game.Gameplay.MainMenu.UI
{
    public class MainMenuUI :MonoBehaviour
    {

        [SerializeField] private Button _startPlayButton;

        private Action _onStartPlay;

        public void Init(Action OnStartPlay)
        {
            _onStartPlay = OnStartPlay;
            _startPlayButton.onClick.AddListener(_onStartPlay.Invoke);
        }

        private void OnDestroy()
        {
            _startPlayButton.onClick.RemoveListener(_onStartPlay.Invoke);
        }
    }
}
