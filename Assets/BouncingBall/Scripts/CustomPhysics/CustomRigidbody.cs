using UnityEngine;

public class CustomRigidbody : MonoBehaviour
{
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _vilocityDamping;

    public Vector3 TestVelocity => _velocityForce;

    private const float fallAcceleration = 9.81f;

    private Vector3 _rotationForce;
    private Vector3 _velocityForce = Vector3.zero;

    private bool _isFall = true;

    private void FixedUpdate()
    {
        // Обновляем позицию и вращение
        transform.position += _velocityForce * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(_rotationForce) * Quaternion.Euler(transform.rotation.eulerAngles);

        ReduceSpeedOfMovement();
        ReduceSpeedOfRotation();
        TryFall();
    }

    public void AddForce(Vector3 direction)
    {
        Move(direction); // Учитываем массу при добавлении силы
        Rotate(new Vector3(direction.z, 0, direction.x * -1));
    }

    public void Move(Vector3 direction)
    {
        _velocityForce += direction / _mass;
    }

    public void Rotate(Vector3 direction)
    {
        _rotationForce += direction / _mass;
    }

    private void ReduceSpeedOfMovement()
    {
        if (Vector3.Distance(_velocityForce, Vector3.zero) > 0.01f)
        {
            _velocityForce = Vector3.Lerp(_velocityForce, Vector3.zero, _vilocityDamping * Time.fixedDeltaTime);
            return;
        }

        _velocityForce = Vector3.zero;
    }

    private void ReduceSpeedOfRotation()
    {
        if (Vector3.Distance(_rotationForce, Vector3.zero) > 0.01f)
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
            _velocityForce.y -= (fallAcceleration * _mass) * Time.fixedDeltaTime; // Учитываем массу при падении
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        CalculateBounceDirection(_velocityForce, normal);

        Vector3 enterDirection = (collision.GetContact(0).point - transform.position).normalized;
        TryStopTheFall(enterDirection, collision);
    }

    private Vector3 CalculateBounceDirection(Vector3 velocity, Vector3 normal)
    {
        var reflectedVelocity = Vector3.Reflect(velocity, normal)  * _vilocityDamping;


        if (reflectedVelocity.y < 1)
        {
            reflectedVelocity.y = 0;
        }

        _velocityForce = reflectedVelocity;
        _rotationForce = new Vector3(reflectedVelocity.z, 0, reflectedVelocity.x * -1);
        return _velocityForce;
    }

    private void OnCollisionExit(Collision collision)
    {
        var exitDirection = (collision.transform.position - transform.position).normalized;

        if (exitDirection.y < -0.1f)
        {
            _isFall = true;
        }
    }

    private void TryStopTheFall(Vector3 enterDirection, Collision collision)
    {
        if (enterDirection.y < -0.1f)
        {
            _isFall = false;

            if (_velocityForce.y < 0.1f)
            {
                _velocityForce.y = 0;
                var newPosition = transform.position;
                newPosition.y = collision.GetContact(0).point.y + transform.localScale.y / 2;
                transform.position = newPosition;
            }
           
        }
    }
}
