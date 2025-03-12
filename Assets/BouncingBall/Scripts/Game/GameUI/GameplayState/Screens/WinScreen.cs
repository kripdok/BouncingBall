using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState.Screen
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private Button _backToMenuButton;

        public void SetExitButton(IObserver<string> observer)
        {
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => observer.OnNext("")).AddTo(this);
        }
    }
}
