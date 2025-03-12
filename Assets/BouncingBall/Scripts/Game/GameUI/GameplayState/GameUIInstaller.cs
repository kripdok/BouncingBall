using BouncingBall.Game.UI.GameplayState.HUD;
using BouncingBall.Game.UI.GameplayState.Screen;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScreens();
            BindPopup();
            BindFactorys();
        }

        private void BindScreens()
        {
            Container.Bind<GameHUD>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/HUD/GameHUD").AsCached();

        }

        private void BindPopup()
        {
            Container.Bind<WinScreen>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/Popups/WinScreen").AsCached();
            Container.Bind<LossScreen>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/Popups/LossScreen").AsCached();
        }

        private void BindFactorys()
        {
            Container.BindFactory<Transform, PlayerHealthCell, PlayerHealthCellFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/HUD/HealthCell").AsCached();
        }
    }
}
