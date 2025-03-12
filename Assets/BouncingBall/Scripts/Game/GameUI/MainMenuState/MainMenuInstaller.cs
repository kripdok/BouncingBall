using BouncingBall.Game.UI.MainMenuState.MVVM;
using BouncingBall.Game.UI.MainMenuState.Screen;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScreens();
            BindFactorys();
        }

        private void BindScreens()
        {
            Container.Bind<LevelSelectionScreen>().FromComponentInNewPrefabResource("Prefabs/UI/MainMenu/Screens/LevelSelection/LevelSelectionScreen").AsCached();
            Container.Bind<MenuScreen>().FromComponentInNewPrefabResource("Prefabs/UI/MainMenu/Screens/LevelSelection/MenuScreen").AsCached();
        }

        private void BindFactorys()
        {
            Container.BindFactory<Transform, LevelSelectionViewModel, LevelSelectionView, LevelSelectionViewFactory>()
                .FromComponentInNewPrefabResource("Prefabs/UI/MainMenu/Screens/LevelSelection/LevelSelectionView").AsCached();
        }
    }
}
