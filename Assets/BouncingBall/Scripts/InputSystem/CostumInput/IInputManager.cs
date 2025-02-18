using UniRx;
using UnityEngine;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public interface IInputManager
    {
        public ReadOnlyReactiveProperty<Vector3> RotationAmount { get; }
        public ReadOnlyReactiveProperty<float> ZScale { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet { get; }
        public ReadOnlyReactiveProperty<float> Angle { get;}
        public ISubject<Unit> InputChange { get; }

        public void EnableControllable();
    }
}
