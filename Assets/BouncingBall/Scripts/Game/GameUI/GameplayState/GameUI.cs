using BouncingBall.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUI : StateUI
    {
        [SerializeField] private Button _backToMenuButton;


        public void Awake()
        {
            _backToMenuButton.onClick.AsObservable().Subscribe(_ => OnExit.OnNext(Unit.Default)).AddTo(this);
        }
    }
}
