using UniRx;
using UnityEngine;

public interface IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; }
    public ReactiveProperty<Vector3> RotationAmount { get; }
    public ReactiveProperty<float> ZScale { get; }

    public void SetRotationAndScale();

    public void TryDisableIsDirectionSet();
}
