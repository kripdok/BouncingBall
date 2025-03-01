namespace BouncingBall.InputSystem.Controller
{
    public interface IInputInteractivityChanger
    {
        public void EnableInput();
        public void DisableInput();
        public void EnableInputSimulator();
        public void DisableInputSimulator();
    }
}
