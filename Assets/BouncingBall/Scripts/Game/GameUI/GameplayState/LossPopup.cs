using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState
{
    public class LossPopup : MonoBehaviour
    {
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _restartButton;

        public void SetExitButton(IObserver<string> observer)
        {
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => observer.OnNext("")).AddTo(this);
        }

        public void SetRestartButton(IObserver<Unit> OnRestart)
        {
            _restartButton.onClick.AsObservable().Subscribe(_ => OnRestart.OnNext(Unit.Default)).AddTo(this);
        }
    }
}
