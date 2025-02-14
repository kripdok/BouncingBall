using UnityEngine;

public class CustomRigidbody : MonoBehaviour
{
    [SerializeField] private float _linearDrag;
    [SerializeField] private float _angularDrag;
    [SerializeField] private float _rayLength = 5f;
    [SerializeField] private float pushForce = 5f; // Сила выталкивания

    private const float fallAcceleration = 9.81f;

    private Vector3 _rotationForce;
    private Vector3 _velocityForce = Vector3.zero;
    private float radius;

    private void Awake()
    {
        radius = GetComponent<Collider>().bounds.extents.x; // Исправлено на использование поля radius
        Debug.Log(radius);
    }

    private void FixedUpdate()
    {
        transform.position += _velocityForce * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(_rotationForce) * Quaternion.Euler(transform.rotation.eulerAngles);

        ReduceSpeedOfMovement();
        ReduceSpeedOfRotation();
        RaycastDown();
    }

    public void AddForce(Vector3 direction)
    {
        _velocityForce += direction;
        _rotationForce += new Vector3(direction.z, 0, direction.x * -1);
    }

    private void ReduceSpeedOfMovement()
    {
        if (Vector3.Distance(_velocityForce, Vector3.zero) > 0.001f)
        {
            _velocityForce = Vector3.Lerp(_velocityForce, Vector3.zero, _linearDrag * Time.fixedDeltaTime);
            return;
        }

        _velocityForce = Vector3.zero;
    }

    private void ReduceSpeedOfRotation()
    {
        if (Vector3.Distance(_rotationForce, Vector3.zero) > 0.001f)
        {
            _rotationForce = Vector3.Lerp(_rotationForce, Vector3.zero, _angularDrag * Time.fixedDeltaTime);
            return;
        }

        _rotationForce = Vector3.zero;
    }

    void RaycastDown()
    {
        if (IsGround())
        {
            _velocityForce.y = 0;
        }
        else
        {
            _velocityForce.y -= fallAcceleration * Time.fixedDeltaTime;
        }
    }

    private bool IsGround()
    {
        Vector3 origin = transform.position;

        return Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, _rayLength);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {

            Vector3 direction = contact.point - transform.position;
            direction.Normalize();
            Debug.Log(direction);
            transform.position = new Vector3(transform.position.x, collision.transform.position.y + collision.transform.localScale.y + _rayLength, transform.position.z);

            _velocityForce.y = 0;

        }

    }
}
