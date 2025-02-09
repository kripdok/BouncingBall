using System;
using Zenject;

namespace BouncingBall.UI
{
    public class StateUIFactory : PlaceholderFactory<UnityEngine.Object, Action, StateUI>
    {
        public override StateUI Create(UnityEngine.Object param, Action action)
        {
            return base.Create(param, action);
        }
    }
}
