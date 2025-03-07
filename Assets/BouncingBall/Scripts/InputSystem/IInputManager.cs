using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem
{
    public interface IInputManager
    {
        public ReadOnlyReactiveProperty<Vector3> Direction { get; }
        public ReadOnlyReactiveProperty<float> DistanceScale { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionActive { get; }
        public ReadOnlyReactiveProperty<float> Angle { get; }
        public ISubject<Unit> InputChange { get; }

        public void EnableControllable();
    }
}
