using BouncingBall.UI.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.UI.Root
{
    public class UIRootHolder : MonoBehaviour, ILoadingWindowController, IAttachStateUI
    {
        [SerializeField] private LoadingWindow _loadingWindow;
        [SerializeField] private Transform _uiSceneContainer;

        public void Awake()
        {
            _loadingWindow.Initialize();
        }

        public async UniTask HideLoadingWindow()
        {
            await _loadingWindow.Hide();
            _loadingWindow.gameObject.SetActive(false);
        }

        public async UniTask ShowLoadingWindow()
        {
            _loadingWindow.gameObject.SetActive(true);
            await _loadingWindow.Show();
        }

        public void AttachStateUI(StateUI sceneUI)
        {
            ClearStateUI();

            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ClearStateUI()
        {
            //TODO - тут происходит уничтожение экранов. Не соответствует ТЗ
            var childCount = _uiSceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}
