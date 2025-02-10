using BouncingBall.Game.UI.MVVM.Level;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState
{
    public class MainMenuInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<Transform, LevelViewModel, LevelView, LevelViewFactory>().FromComponentInNewPrefabResource("Prefabs/UI/Containers/MenuButton").AsCached();
        }
    }
}
