using UnityEngine;

public class CustomRigidbody : MonoBehaviour
{
    [SerializeField] private float _mass = 1f;
    [SerializeField, Range(0, 10)] private float _vilocityDamping;

    private const float FallAcceleration = 9.81f;
    private const float MinYvelocity = 1;
    private const float MinDistanceToReduceSpeed = 0.01f;

    public Vector3 TestVelocity => _velocityForce;

    private Vector3 _rotationForce;
    private Vector3 _velocityForce;
    private bool _isFall = true;

    private void FixedUpdate()
    {
        transform.position += _velocityForce * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(_rotationForce) * Quaternion.Euler(transform.rotation.eulerAngles);

        ReduceSpeedOfMovement();
        ReduceSpeedOfRotation();
        TryFall();
    }
    private void OnCollisionEnter(Collision collision)
    {
        CalculateBounceDirection(collision);
        TryStopTheFall(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        TryStartTheFall(collision);
    }

    public void AddForce(Vector3 direction)
    {
        Move(direction);
        Rotate(new Vector3(direction.z, 0, direction.x * -1));
    }

    public void Move(Vector3 direction)
    {
        _velocityForce += direction / _mass * Time.fixedDeltaTime;
    }

    public void Rotate(Vector3 direction)
    {
        _rotationForce += direction / _mass * Time.fixedDeltaTime;
    }

    private void ReduceSpeedOfMovement()
    {
        if (Vector3.Distance(_velocityForce, Vector3.zero) > MinDistanceToReduceSpeed)
        {
            _velocityForce = Vector3.Lerp(_velocityForce, Vector3.zero, _vilocityDamping * Time.fixedDeltaTime);
            return;
        }

        _velocityForce = Vector3.zero;
    }

    private void ReduceSpeedOfRotation()
    {
        if (Vector3.Distance(_rotationForce, Vector3.zero) > MinDistanceToReduceSpeed)
        {
            _rotationForce = Vector3.Lerp(_rotationForce, Vector3.zero, _vilocityDamping * Time.fixedDeltaTime);
            return;
        }

        _rotationForce = Vector3.zero;
    }

    private void TryFall()
    {
        if (_isFall)
        {
            _velocityForce.y -= FallAcceleration * _mass * Time.fixedDeltaTime;
        }
    }

    private void CalculateBounceDirection(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;

        _velocityForce = Vector3.Reflect(_velocityForce, normal);
        _velocityForce.y *= _vilocityDamping;
        _rotationForce = new Vector3(_velocityForce.z, 0, _velocityForce.x * -1);
    }

    private void TryStopTheFall(Collision collision)
    {
        var enterDirection = (collision.GetContact(0).point - transform.position).normalized;
        float minYDirection = -0.1f;

        if (enterDirection.y < minYDirection)
        {
            _isFall = false;

            if (_velocityForce.y < MinYvelocity)
            {
                _velocityForce.y = 0;
                var newPosition = transform.position;
                newPosition.y = collision.GetContact(0).point.y + transform.localScale.y / 2;
                transform.position = newPosition;
            }
        }
    }

    private void TryStartTheFall(Collision collision)
    {
        var exitDirection = (collision.transform.position - transform.position).normalized;
        float minYDirection = -0.1f;

        if (exitDirection.y < minYDirection)
        {
            _isFall = true;
        }
    }
}
