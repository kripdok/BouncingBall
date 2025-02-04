using BouncingBall.Scripts.Game.Gameplay.MainMenu.UI;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        private const string LevelId = "0";
        private const string UIPrefabPathc = "Prefabs/UI/Containers/MainMenuUI";

        private readonly LevelLoader _levelLoader;
        private readonly IAttachStateUI _attachStateUI;
        private readonly IPrefabLoadStrategy _prefabLoadStrategy;

        public GameplayBootstrap(LevelLoader levelLoader, IAttachStateUI attachStateUI, IPrefabLoadStrategy prefabLoadStrategy)
        {
            _levelLoader = levelLoader;
            _attachStateUI = attachStateUI;
            _prefabLoadStrategy = prefabLoadStrategy;
            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            _levelLoader.LoadLevel(LevelId);
            CreateMainMenuUI();
        }

        private void CreateMainMenuUI()
        {
            var prefabMainMenuUI = _prefabLoadStrategy.LoadPrefab<MainMenuUI>(UIPrefabPathc);
            var mainMenuUI = GameObject.Instantiate(prefabMainMenuUI);
            _attachStateUI.AttachStateUI(mainMenuUI.gameObject);
        }
    }
}
