using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public class StateUIFactory : PlaceholderFactory<UnityEngine.Object,StateUI>
    {
        public override StateUI Create(Object param)
        {
            return base.Create(param);
        }
    }
}
