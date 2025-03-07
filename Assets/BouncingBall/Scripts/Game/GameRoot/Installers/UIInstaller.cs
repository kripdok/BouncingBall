using BouncingBall.UI;
using BouncingBall.UI.Root;
using Zenject;

namespace Assets.BouncingBall.Scripts.Game.GameRoot.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindLoadingWindow();
            BindStateUIFactory();
        }

        private void BindLoadingWindow()
        {
            Container.BindInterfacesTo<UIRootHolder>()
                .FromComponentInNewPrefabResource("Prefabs/UI/UIRoot")
                .AsSingle()
                .NonLazy();
        }

        private void BindStateUIFactory()
        {
            Container.BindFactory<UnityEngine.Object, StateUI, StateUIFactory>()
                .FromFactory<PrefabFactory<StateUI>>();
        }
    }
}
