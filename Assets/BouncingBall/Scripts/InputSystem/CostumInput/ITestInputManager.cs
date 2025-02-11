using UniRx;

namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public interface ITestInputManager
    {
        public ReadOnlyReactiveProperty<float> RotationAmount { get; }
        public ReadOnlyReactiveProperty<float> ZScale { get; }
        public ReadOnlyReactiveProperty<bool> IsDirectionSet2 { get; }
    }
}
