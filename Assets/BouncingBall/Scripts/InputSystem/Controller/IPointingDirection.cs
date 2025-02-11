using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Controller
{
    public interface IPointingDirection
    {
        public ReadOnlyReactiveProperty<Vector2> PointerLocation { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; }

        public ISubject<Vector2> Position { get; }
    }
}
