using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class CoinCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinCountText;
        [Header("Animation")]
        [SerializeField] private float _scaleMultiplier;
        [SerializeField, Range(0, 0.5f)] private float _animationPlayerTime;

        private CoinCounterViewModel _viewModel;
        private Vector3 _targetScale;
        private Vector3 _defaultScale;
        private bool _isEnabled;

        public void Init(CoinCounterViewModel viewModel)
        {
            _viewModel = viewModel;
            _coinCountText.text = _viewModel.CoinCount.Value.ToString();
            _viewModel.CoinCount.Skip(1).Subscribe(UpdateCoinCountWithAnimation).AddTo(this);

            _targetScale = _coinCountText.transform.localScale * _scaleMultiplier;
            _defaultScale = _coinCountText.transform.localScale;
            _isEnabled = true;
        }

        private void OnDestroy()
        {
            _isEnabled = false;
        }

        private async void UpdateCoinCountWithAnimation(int amount)
        {
            if (!_isEnabled)
                return;

            _coinCountText.text = amount.ToString();

            await AnimateScaleUp();
            await AnimateScaleDown();
        }

        private async UniTask AnimateScaleUp()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationPlayerTime && _isEnabled)
            {
                float lerpT = elapsedTime / _animationPlayerTime;
                _coinCountText.transform.localScale = Vector3.Lerp(_defaultScale, _targetScale, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private async UniTask AnimateScaleDown()
        {
            Vector3 currentScale = _coinCountText.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < _animationPlayerTime && _isEnabled)
            {
                float lerpT = elapsedTime / _animationPlayerTime;
                _coinCountText.transform.localScale = Vector3.Lerp(currentScale, _defaultScale, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            if (_isEnabled)
            {
                _coinCountText.transform.localScale = _defaultScale;
            }
        }
    }
}
