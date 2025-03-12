using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState.Screen
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _levelSelectionButton;

        [Inject] private LevelSelectionScreen _levelSelectionScreen;

        private void Awake()
        {
            _levelSelectionButton.onClick.AsObservable().Subscribe(_ => OpenLevelSelectionScreen()).AddTo(this);
        }

        private void OpenLevelSelectionScreen()
        {
            gameObject.SetActive(false);
            _levelSelectionScreen.gameObject.SetActive(true);
        }
    }
}
