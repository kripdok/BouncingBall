using UnityEngine;

namespace BouncingBall.CustomPhysics
{
    public class CustomRigidbody : MonoBehaviour
    {
        [SerializeField] private Transform _body;
        [SerializeField] private float _mass = 1f;
        [SerializeField, Range(0, 10)] private float _vilocityDamping;

        private const float FallAcceleration = 9.81f;
        private const float MinYvelocity = 1;
        private const float MinDistanceToReduceSpeed = 0.01f;

        public Vector3 TestVelocity;

        private Vector3 _rotationForce;
        public Vector3 _velocityForce;
        private bool _isFall = true;
        private ContactPoint _lastContact;

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
            CalculateBounceDirection(_lastContact);
            TryStopTheFall(_lastContact);

            //TODO - здесь надо сделать проверку на то, пересекается ли объект с точкой контакта
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
            TestVelocity = _velocityForce;
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

        private void CalculateBounceDirection(ContactPoint contact)
        {
            Vector3 normal = contact.normal;
            _velocityForce = Vector3.Reflect(_velocityForce, normal);
            TestVelocity = _velocityForce;
            //_velocityForce.y *= _vilocityDamping;
            _velocityForce = Vector3.zero;
            _rotationForce = new Vector3(_velocityForce.z, 0, _velocityForce.x * -1);


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

        private void TryStartTheFall(Collision collision)
        {
            var exitDirection = (_lastContact.point - transform.position).normalized;
            float minYDirection = -0.1f;

            if (exitDirection.y < minYDirection)
            {
                _isFall = true;
            }
        }
    }
}