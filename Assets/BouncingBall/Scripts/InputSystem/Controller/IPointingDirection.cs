﻿using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Controller
{
    public interface IPointingDirection
    {
        public ReadOnlyReactiveProperty<Vector3> PointerLocation { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; }
    }
}
