using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.CustomPhysics
{
    public class CustomRigidbody : MonoBehaviour
    {
        private const float FallAcceleration = 9.81f;
        private const float MinYVelocity = 1f;
        private const float MinDistanceToReduceSpeed = 0.01f;
        private const float MinYDirectionThreshold = -0.1f;

        [SerializeField] private Transform _body;
        [SerializeField] private float _mass = 1f;
        [SerializeField, Range(0, 10)] private float _velocityDamping;

        public Vector3 VelocityForce;
        public Vector3 RotationForce;
        private ContactPoint _lastCollisionContact;
        private bool _isFalling = true;
        private bool _isCollisionResponseAvailable = true;

        public void Reset()
        {
            _isFalling = true;
            VelocityForce = Vector3.zero;
            RotationForce = Vector3.zero;
        }

        private void FixedUpdate()
        {
            ChangeRotateAndPosition();

            VelocityForce = ReduceSpeed(VelocityForce);
            RotationForce = ReduceSpeed(RotationForce);

            TryFall();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _lastCollisionContact = collision.GetContact(0);
            StopFallingIfNeeded(_lastCollisionContact);

            if (_isCollisionResponseAvailable)
            {
                HandleCollisionResponse(collision);
                StartWaitingForCollisionReaction();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            StartFallingIfNeeded(collision);
        }

        public void AddForce(Vector3 direction)
        {
            Move(direction);
            Rotate(new Vector3(direction.z, 0, direction.x * -1));
        }

        public void Move(Vector3 direction)
        {
            VelocityForce += direction / _mass;
        }

        public void Rotate(Vector3 direction)
        {
            RotationForce += direction / _mass;
        }

        private void ChangeRotateAndPosition()
        {
            transform.position += VelocityForce * Time.fixedDeltaTime;
            _body.rotation = Quaternion.Euler(RotationForce) * Quaternion.Euler(_body.rotation.eulerAngles);
        }

        private Vector3 ReduceSpeed(Vector3 force)
        {
            if (Vector3.Distance(force, Vector3.zero) > MinDistanceToReduceSpeed)
            {
                return Vector3.Lerp(force, Vector3.zero, _velocityDamping * Time.fixedDeltaTime);
            }

            return Vector3.zero;
        }

        private void TryFall()
        {
            if (_isFalling)
            {
                VelocityForce.y -= FallAcceleration * _mass * Time.fixedDeltaTime;
            }
        }

        private void StopFallingIfNeeded(ContactPoint contact)
        {
            var enterDirection = (contact.point - transform.position).normalized;

            if (enterDirection.y < MinYDirectionThreshold)
            {
                _isFalling = false;

                if (VelocityForce.y < MinYVelocity)
                {
                    VelocityForce.y = 0;
                    var newPosition = transform.position;
                    newPosition.y = contact.point.y + transform.localScale.y / 2;
                    transform.position = newPosition;
                }
            }
        }

        private async void HandleCollisionResponse(Collision collision)
        {
            Vector3 normal = _lastCollisionContact.normal;
            UpdateRotation(normal);

            var newVelocity = Vector3.Reflect(VelocityForce, normal);
            newVelocity.y = 0;
            RotationForce = new Vector3(newVelocity.z, 0, newVelocity.x * -1);
            VelocityForce = newVelocity;
        }

        private void UpdateRotation(Vector3 eulerAngle)
        {
            var sumRotation = _body.rotation;
            transform.rotation = Quaternion.LookRotation(eulerAngle);
            _body.rotation = sumRotation;
        }

        private void StartFallingIfNeeded(Collision collision)
        {
            var exitDirection = (_lastCollisionContact.point - transform.position).normalized;

            if (exitDirection.y < MinYDirectionThreshold)
            {
                _isFalling = true;
            }
        }

        private async void StartWaitingForCollisionReaction()
        {
            _isCollisionResponseAvailable = false;
            await UniTask.Yield();
            _isCollisionResponseAvailable = true;
        }
    }
}