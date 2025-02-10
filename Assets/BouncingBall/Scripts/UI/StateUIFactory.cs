using Zenject;

namespace BouncingBall.UI
{
    public class StateUIFactory : PlaceholderFactory<UnityEngine.Object, StateUI>
    {
        public override StateUI Create(UnityEngine.Object param)
        {
            return base.Create(param);
        }
    }
}
