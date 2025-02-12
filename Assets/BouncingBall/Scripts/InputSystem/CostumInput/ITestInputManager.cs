using UniRx;
using UnityEngine;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public interface ITestInputManager
    {
        public ReadOnlyReactiveProperty<Vector3> RotationAmount { get; }
        public ReadOnlyReactiveProperty<float> ZScale { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet2 { get; }
    }
}
