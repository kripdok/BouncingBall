namespace BouncingBall.InputSystem
{
    public interface IInputInteractivityChanger
    {
        public void EnableInput();
        public void DisableInput();
        public void EnableSimulatedInput();
        public void DisableSimulatedInput();
    }
}
