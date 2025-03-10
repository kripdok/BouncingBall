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
            InitScreen();
            InitFactorys();
        }

        private void InitScreen()
        {
            Container.Bind<LevelSelectionScreen>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/LevelSelectionScreen").AsCached();
        }

        private void InitFactorys()
        {
            Container.BindFactory<Transform, LevelSelectionViewModel, LevelSelectionView, LevelSelectionViewFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/MenuButton").AsCached();
        }
    }
}
