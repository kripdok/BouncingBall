using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.CustomPhysics
{
    public class CustomRigidbody : MonoBehaviour
    {
        [SerializeField] private Transform _body;
        [SerializeField] private float _mass = 1f;
        [SerializeField, Range(0, 10)] private float _vilocityDamping;
        [SerializeField, Range(0, 10)] private float _maximumCompression;
        [SerializeField, Range(0, 1)] private float _compressionDuration;

        private const float FallAcceleration = 9.81f;
        private const float MinYvelocity = 1;
        private const float MinDistanceToReduceSpeed = 0.01f;

        public Vector3 TestVelocity;
        public Vector3 _velocityForce;

        private Vector3 _rotationForce;
        private bool _isFall = true;
        private ContactPoint _lastContact;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
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
            ReactToCollision(collision);
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
            _rotationForce = new Vector3(newVelocity.z, 0, newVelocity.x * -1);

            await CompressScale(newVelocity);
            await UnclenchScale();

            TestVelocity = _velocityForce;
        }

        private void ChangeRotate(Vector3 eulerAngle)
        {
            var sumRotation = _body.rotation;
            transform.rotation = Quaternion.LookRotation(eulerAngle);
            _body.rotation = sumRotation;
        }

        private async UniTask CompressScale(Vector3 newVelocity)
        {
            var powerCompression = GetVelocityForcePowerCompression();
            var compressionScale = GetCinoressScale(powerCompression);

            float elapsedTime = 0f;

            while (elapsedTime < _compressionDuration)
            {
                float lerpT = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, compressionScale, lerpT);
                _velocityForce = Vector3.Lerp(_velocityForce, newVelocity, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private async UniTask UnclenchScale()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _compressionDuration)
            {
                float lerpT = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            transform.localScale = Vector3.one;
        }

        private float GetVelocityForcePowerCompression()
        {
            var number = Vector3.Distance(Vector3.zero, _velocityForce);
            return Mathf.Clamp(number, 0, _maximumCompression);
        }

        private Vector3 GetCinoressScale(float powerCompression)
        {
            var scale = transform.localScale;
            scale.z = scale.z - (powerCompression / 10);
            return scale;
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