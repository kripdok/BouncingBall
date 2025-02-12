using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class MouseInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<float> RotationAmount { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }


    private float _rotationSpeed = 10f;
    private float _scaleSpeed = 5f;

    private bool _isCooldown;



    private Vector2 mousePosition;
    private bool _isDirectionSet = true;

    public MouseInputDevice()
    {
        _isCooldown = false;
        IsDirectionSet = new();
        RotationAmount = new();
        ZScale = new();
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


    public void SetRotationAndScale()
    {
        if (_isCooldown)
            return;

       
    }

    public void TryDisableIsDirectionSet()
    {
        
    }
}

