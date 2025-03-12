using BouncingBall.Game.UI.MainMenuState.Screen;
using BouncingBall.UI;
using UniRx;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState
{
    public class MainMenuUI : StateUI
    {
        [Inject] private LevelSelectionScreen _levelSelectionScreen;
        [Inject] private MenuScreen _menuScreen;

        private void Awake()
        {
            _menuScreen.gameObject.SetActive(true);
            _levelSelectionScreen.gameObject.SetActive(false);

            _levelSelectionScreen.SetLevelRequested.Subscribe(StartLevelCommand).AddTo(this);
        }

        private void StartLevelCommand(string levelName)
        {
            ExitRequested.OnNext(levelName);
        }
    }
}
