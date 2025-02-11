using UnityEngine;

public class KeyboardInputDevice: IInputDevice
{

    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector2(horizontal, vertical);
    }

    public bool GetActionInput()
    {
        return Input.GetButton("Jump");
    }
}
