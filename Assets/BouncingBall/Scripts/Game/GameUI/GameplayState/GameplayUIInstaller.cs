using BouncingBall.Game.UI.GameplayState;
using UnityEngine;
using Zenject;

namespace Assets.BouncingBall.Scripts.Game.GameUI.GameplayState
{
    public class GameplayUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Transform,PlayerHealthCell,PlayerHealthCellFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Gameplay/HealthCell").AsCached();
        }
    }
}
