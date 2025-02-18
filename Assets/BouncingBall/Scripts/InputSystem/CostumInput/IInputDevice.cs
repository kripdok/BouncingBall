using UniRx;
using UnityEngine;

public interface IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; }
    public ReactiveProperty<Vector3> Direction { get; }
    public ReactiveProperty<float> ZScale { get; }
    public ReactiveProperty<float> Angle { get; }

    public void SetRotationAndScale();

    public void TryDisableIsDirectionSet();
}
