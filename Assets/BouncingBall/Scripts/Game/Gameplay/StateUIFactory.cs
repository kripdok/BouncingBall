using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public class StateUIFactory : PlaceholderFactory<UnityEngine.Object,Action,StateUI>
    {
        public override StateUI Create(UnityEngine.Object param, Action action)
        {
            return base.Create(param, action);
        }
    }
}
