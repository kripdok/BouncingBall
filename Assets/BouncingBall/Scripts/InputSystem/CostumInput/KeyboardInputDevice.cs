using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class KeyboardInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<Vector3> Direction { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }
    public ReactiveProperty<float> Angle { get; private set; }

    private float _rotationSpeed = 50f;
    private float _scaleSpeed = 5f;

    private bool _isCooldown;

    public KeyboardInputDevice()
    {
        _isCooldown = false;
        IsDirectionSet = new();
        Direction = new();
        ZScale = new();
        Angle = new();

    }

    public void SetRotationAndScale()
    {
        if (_isCooldown)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        TryCalculateDirection(horizontal);
        TryCalculationScaleZ(vertical);

        if (horizontal != 0 || vertical != 0)
        {
            IsDirectionSet.Value = true;
            UpdateDirection();
        }
    }

    public void TryDisableIsDirectionSet()
    {
        if (Input.GetButton("Jump") && IsDirectionSet.Value)
        {
            IsDirectionSet.Value = false;
            EnableCooldown();
        }
    }

    private void TryCalculateDirection(float horizontal)
    {
        if (horizontal != 0f)
        {
            Angle.Value += _rotationSpeed * Time.deltaTime * (horizontal > 0 ? 1 : -1);
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

    private async void EnableCooldown()
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(1);
        _isCooldown = false;
    }

    private void UpdateDirection()
    {
        float angleInRadians = (Angle.Value + 90) * Mathf.Deg2Rad;
        float distance = ZScale.Value;

        float x = distance * Mathf.Cos(angleInRadians) * -1;
        float z = distance * Mathf.Sin(angleInRadians);

        var direction = new Vector3(x, 0, z);
        Direction.Value = direction.normalized;
    }
}
