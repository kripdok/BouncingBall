using UniRx;
using UnityEngine;

namespace BouncingBall.InputSystem.Device
{
    public interface IInputDevice
    {
        public ReactiveProperty<bool> IsDirectionActive { get; }
        public ReactiveProperty<Vector3> Direction { get; }
        public ReactiveProperty<float> DistanceScale { get; }
        public ReactiveProperty<float> Angle { get; }

        public void UpdateRotationAndScale();

        public void UpdateDirectionAndScale();

        public void Reset();
    }
}
