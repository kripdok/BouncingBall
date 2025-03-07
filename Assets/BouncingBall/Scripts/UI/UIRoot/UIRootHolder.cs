using BouncingBall.UI.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.UI.Root
{
    public class UIRootHolder : MonoBehaviour, ILoadingWindowController, IAttachStateUI
    {
        [SerializeField] private LoadingWindow _loadingWindow;
        [SerializeField] private Transform _uiSceneContainer;
        public StateUI StateUI { get; private set; }

        public void Awake()
        {
            _loadingWindow.Initialize();
        }

        public async UniTask HideLoadingScreen()
        {
            await _loadingWindow.Hide();
            _loadingWindow.gameObject.SetActive(false);
        }

        public async UniTask ShowLoadingScreen()
        {
            _loadingWindow.gameObject.SetActive(true);
            await _loadingWindow.Show();
        }

        public void AttachStateUI(StateUI sceneUI)
        {
            ClearStateUI();
            StateUI = sceneUI;
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
