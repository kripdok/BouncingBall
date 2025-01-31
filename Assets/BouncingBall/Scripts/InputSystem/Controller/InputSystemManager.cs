namespace BouncingBall.Scripts.InputSystem.Controller
{
    public class InputSystemManager
    {
        private readonly InputSystemActions _inputActions;

        //TODO - подумать где будет отключаться управление. Для этого нужен Интерфейс?
        public InputSystemManager(InputSystemActions inputActions)
        {
            _inputActions = inputActions;
        }

        public void EnableInputSystam()
        {
            _inputActions.Enable();
        }


        public void DisableInputSystam()
        {
            _inputActions.Disable();
        }
    }
}
