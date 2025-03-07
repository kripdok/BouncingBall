namespace BouncingBall.InputSystem.Device
{
    public interface IControllable
    {
        public bool IsControllable { get; }
        public void EnableControllable();
    }
}
