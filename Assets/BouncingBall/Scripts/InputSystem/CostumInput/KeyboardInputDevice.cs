using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class KeyboardInputDevice : IInputDevice
{

    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<float> RotationAmount { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }


    private float _rotationSpeed = 10f;
    private float _scaleSpeed = 5f;

    private bool _isCooldown;

    public KeyboardInputDevice()
    {
        _isCooldown = false;
        IsDirectionSet = new();
        RotationAmount = new();
        ZScale = new();
    }

    public void SetRotationAndScale()
    {
        if (_isCooldown)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f)
        {
            RotationAmount.Value = _rotationSpeed * Time.deltaTime * (horizontal > 0 ? 1 : -1);
        }

        if (vertical != 0f)
        {
            ZScale.Value = _scaleSpeed * Time.deltaTime * (vertical > 0 ? 1 : -1);
        }

        if (horizontal != 0 || vertical != 0)
        {
            IsDirectionSet.Value = true;
        }
    }

    public void TryDisableIsDirectionSet()
    {
        if (Input.GetButton("Jump") & IsDirectionSet.Value)
        {
            IsDirectionSet.Value = false;
            EnableCoolduwn();
        }
    }

    private async void EnableCoolduwn()
    {
        _isCooldown = true;

        await UniTask.WaitForSeconds(1);

        _isCooldown = false;
    }
}
