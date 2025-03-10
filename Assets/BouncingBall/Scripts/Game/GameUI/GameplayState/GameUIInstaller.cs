using BouncingBall.Game.UI.GameplayState.HUD;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class GameUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Transform, PlayerHealthCell, PlayerHealthCellFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/HealthCell").AsCached();
        }
    }
}
