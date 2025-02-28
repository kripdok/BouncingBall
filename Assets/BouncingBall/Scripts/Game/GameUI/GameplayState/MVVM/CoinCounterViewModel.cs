using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class CoinCounterViewModel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _count;
        [Header("Animation")]
        [SerializeField] private float _scaleMultiplier;
        [SerializeField, Range(0, 0.5f)] private float _animationPlayerTime;

        private CoinCounterView _view;
        private Vector3 _animationScale;
        private Vector3 _defoltScale;

        public void Init(CoinCounterView view)
        {
            _view = view;
            _count.text = _view.CoinCount.Value.ToString();
            _view.CoinCount.Skip(1).Subscribe(PlayReplenishmentAnimation).AddTo(this);
            _animationScale = _count.transform.localScale * _scaleMultiplier;
            _defoltScale = _count.transform.localScale;
        }

        private async void PlayReplenishmentAnimation(int amount)
        {
            var exitTime = 0f;
            _count.text = amount.ToString();

            while (exitTime < _animationPlayerTime)
            {
                float lerpT = exitTime / _animationPlayerTime;
                _count.transform.localScale = Vector3.Lerp(_defoltScale, _animationScale, lerpT);
                exitTime += Time.deltaTime;
                await UniTask.Yield();
            }

            var scale = _count.transform.localScale;
            exitTime = 0;

            while (exitTime < _animationPlayerTime)
            {
                float lerpT = exitTime / _animationPlayerTime;
                _count.transform.localScale = Vector3.Lerp(scale, _defoltScale, lerpT);
                exitTime += Time.deltaTime;
                await UniTask.Yield();
            }

            _count.transform.localScale = _defoltScale;
        }
    }
}
