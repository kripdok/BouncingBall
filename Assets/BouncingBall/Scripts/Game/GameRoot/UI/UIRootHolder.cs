using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Game.GameRoot.UI
{
    public class UIRootHolder : MonoBehaviour, ILoadingWindowController
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
        }

        public async UniTask ShowLoadingWindow()
        {
            await _loadingWindow.Show();
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();

            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ClearSceneUI()
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
