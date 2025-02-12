using UniRx;

public interface IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; }
    public ReactiveProperty<float> RotationAmount { get; }
    public ReactiveProperty<float> ZScale { get; }

    public void SetRotationAndScale();

    public void TryDisableIsDirectionSet();
}
