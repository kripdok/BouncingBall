using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class KeyboardInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<Vector3> Direction { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }


    private float _rotationSpeed = 20f;
    private float _scaleSpeed = 5f;

    private bool _isCooldown;

    public KeyboardInputDevice()
    {
        _isCooldown = false;
        IsDirectionSet = new();
        Direction = new();
        ZScale = new();
    }

    public void SetRotationAndScale()
    {
        if (_isCooldown)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        TryÑalculatóDirection(horizontal);
        TryCalculationScaleZ(vertical);

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

    private void TryÑalculatóDirection(float horizontal)
    {
        if (horizontal != 0f)
        {
            float rotationChange = _rotationSpeed * Time.deltaTime * (horizontal > 0 ? 1 : -1);
            Direction.Value += new Vector3(0, rotationChange, 0);
        }
    }

    private void TryCalculationScaleZ(float vertical)
    {
        if (vertical != 0f)
        {
            var scaleZ = ZScale.Value;
            scaleZ += _scaleSpeed * Time.deltaTime * (vertical > 0 ? 1 : -1);
            scaleZ = Mathf.Clamp(scaleZ, 0, 3f);
            ZScale.Value = scaleZ;
        }
    }

    private async void EnableCoolduwn()
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(1);
        _isCooldown = false;
    }

    public void SetTest()
    {
       
    }
}