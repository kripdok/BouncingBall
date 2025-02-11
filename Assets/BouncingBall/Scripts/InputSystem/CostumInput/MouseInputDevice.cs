using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseInputDevice : IInputDevice
{
    private Vector2 mousePosition;
    private bool _isDirectionSet = true;


    public bool GetActionInput()
    {
        return Input.GetButton("Fire1");
    }

    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        return new Vector2(horizontal, vertical);
    }

    private async void StartMove()
    {
        _isDirectionSet = true;
        await Move();

        //Подает сигнал для включения указателя
    }

    private void StopMove()
    {
        _isDirectionSet = false;
    }

    private async UniTask Move()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        do
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float distance))
            {
                mousePosition = ray.GetPoint(distance);
            }

            await UniTask.Yield();
        }
        while (_isDirectionSet);
    }
}

