namespace Assets.BouncingBall.Scripts.InputSystem.CostumInput
{
    public interface IControllable
    {
        public bool IsControllable { get; }
        public void EnableControllable();
    }
}
