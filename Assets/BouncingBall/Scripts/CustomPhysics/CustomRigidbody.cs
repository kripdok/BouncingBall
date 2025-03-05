using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.CustomPhysics
{
    public class CustomRigidbody : MonoBehaviour
    {
        private const float FallAcceleration = 9.81f;
        private const float MinYvelocity = 1;
        private const float MinDistanceToReduceSpeed = 0.01f;

        [SerializeField] private Transform _body;
        [SerializeField] private float _mass = 1f;
        [SerializeField, Range(0, 10)] private float _vilocityDamping;


        public Vector3 TestVelocity;
        public Vector3 _velocityForce;

        private Vector3 _rotationForce;
        private bool _isFall = true;
        private ContactPoint _lastContact;

        private bool _isTest = true;

        public void Reset()
        {
            _isFall = true;
            _velocityForce = Vector3.zero;
            _rotationForce = Vector3.zero;
        }

        private void FixedUpdate()
        {
            transform.position += _velocityForce * Time.fixedDeltaTime;
            _body.rotation = Quaternion.Euler(_rotationForce) * Quaternion.Euler(_body.rotation.eulerAngles);

            ReduceSpeedOfMovement();
            ReduceSpeedOfRotation();
            TryFall();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _lastContact = collision.GetContact(0);
            TryStopTheFall(_lastContact);

            if (_isTest)
            {
                ReactToCollision(collision);
                Test();
            }

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
            _velocityForce += direction / _mass;
            TestVelocity = _velocityForce;
        }

        public void Rotate(Vector3 direction)
        {
            _rotationForce += direction / _mass;
        }

        private void ReduceSpeedOfMovement()
        {
            if (Vector3.Distance(_velocityForce, Vector3.zero) > MinDistanceToReduceSpeed)
            {
                _velocityForce = Vector3.Lerp(_velocityForce, Vector3.zero, _vilocityDamping * Time.fixedDeltaTime);
                return;
            }

            _velocityForce = Vector3.zero;
            TestVelocity = _velocityForce;
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

        private void TryStopTheFall(ContactPoint contact)
        {
            var enterDirection = (contact.point - transform.position).normalized;
            float minYDirection = -0.1f;

            if (enterDirection.y < minYDirection)
            {
                _isFall = false;

                if (_velocityForce.y < MinYvelocity)
                {
                    _velocityForce.y = 0;
                    var newPosition = transform.position;
                    newPosition.y = contact.point.y + transform.localScale.y / 2;
                    transform.position = newPosition;
                }
            }
        }

        private async void ReactToCollision(Collision collision)
        {
            Vector3 normal = _lastContact.normal;
            ChangeRotate(normal);

            var newVelocity = Vector3.Reflect(_velocityForce, normal);
            Debug.Log(newVelocity);
            newVelocity.y = 0;
            _rotationForce = new Vector3(newVelocity.z, 0, newVelocity.x * -1);
            _velocityForce = newVelocity;

            TestVelocity = _velocityForce;
        }

        private void ChangeRotate(Vector3 eulerAngle)
        {
            var sumRotation = _body.rotation;
            transform.rotation = Quaternion.LookRotation(eulerAngle);
            _body.rotation = sumRotation;
        }

        private void TryStartTheFall(Collision collision)
        {
            var exitDirection = (_lastContact.point - transform.position).normalized;
            float minYDirection = -0.1f;

            if (exitDirection.y < minYDirection)
            {
                _isFall = true;
            }
        }

        private async void Test()
        {
            _isTest = false;
            await UniTask.WaitForSeconds(0.1f);
            _isTest = true;
        }
    }
}