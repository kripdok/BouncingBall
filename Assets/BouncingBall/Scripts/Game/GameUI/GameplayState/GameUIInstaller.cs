using BouncingBall.Game.UI.GameplayState.HUD;
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
            Container.Bind<WinPopup>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/Popups/WinPopup").AsCached();
            Container.Bind<LossPopup>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/Popups/LossPopup").AsCached();
        }

        private void BindFactorys()
        {
            Container.BindFactory<Transform, PlayerHealthCell, PlayerHealthCellFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/HUD/HealthCell").AsCached();
        }
    }
}
