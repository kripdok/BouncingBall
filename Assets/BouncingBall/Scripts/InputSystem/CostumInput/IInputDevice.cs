using UnityEngine;

public interface IInputDevice
{
    public Vector2 GetMovementInput();
    public bool GetActionInput();
}
