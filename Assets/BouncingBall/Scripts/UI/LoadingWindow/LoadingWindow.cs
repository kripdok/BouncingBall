using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.UI.Loading
{
    public class LoadingWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingWindowCanvasGroup;

        public void Initialize()
        {
            _loadingWindowCanvasGroup.alpha = 1.0f;
        }

        public async UniTask Hide()
        {
            float duration = 0.1f;
            await UniTask.WaitForSeconds(1f);

            while (_loadingWindowCanvasGroup.alpha != 0)
            {
                _loadingWindowCanvasGroup.alpha -= duration;
                await UniTask.Yield();
            }

        }

        public async UniTask Show()
        {
            float duration = 0.1f;


            while (_loadingWindowCanvasGroup.alpha != 1)
            {
                _loadingWindowCanvasGroup.alpha += duration;
                await UniTask.Yield();
            }

        }
    }
}
