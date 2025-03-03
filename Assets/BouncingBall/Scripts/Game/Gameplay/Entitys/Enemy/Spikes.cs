using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Spikes : AbstractEnemy
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _deathAnimationDurationduration = 0.5f;
        [SerializeField] private float _rayLength = 10f;

        private Vector3 _defoltScale;

        private Vector3 _firstMovePoint;
        private Vector3 _secondMovePoint;

        public override void Reset()
        {
            base.Reset();
            transform.localScale = _defoltScale;
            Collider.enabled = true;
            _firstMovePoint = GetRaycastPoint(Vector3.left);
            _secondMovePoint = GetRaycastPoint(Vector3.right);
            Move();
        }

        protected override void Awake()
        {
            base.Awake();
            _defoltScale = transform.localScale;
            Reset();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (IsCollisionPerpendicular(collision))
                {
                    PlayDisappearingAnimation();
                }
                else
                {
                    damageable.TakeDamage(1);
                }
            }
        }

        protected override bool IsCollisionPerpendicular(Collision collision)
        {
            Vector3 enemyNormal = collision.contacts[0].normal;

            Vector3 localNormal = transform.InverseTransformDirection(enemyNormal);

            var angle = Mathf.Atan2(localNormal.x, localNormal.z) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            bool one = (angle >= 350 || angle <= 5);
            bool two = (angle >= 170 && angle <= 190);

            return one || two;
        }

        private async void PlayDisappearingAnimation()
        {
            Collider.enabled = false;

            Vector3 initialPosition = transform.position;
            Vector3 initialScale = _defoltScale;

            float elapsedTime = 0f;

            while (elapsedTime < _deathAnimationDurationduration)
            {
                float t = elapsedTime / _deathAnimationDurationduration;
                transform.position = initialPosition + new Vector3(0, t, 0);
                transform.Rotate(Vector3.up, 360 * Time.deltaTime / _deathAnimationDurationduration);
                transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            gameObject.SetActive(false);
        }

        private Vector3 GetRaycastPoint(Vector3 direction)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, _rayLength))
            {
                return hit.point;
            }
            else
            {
                return transform.position + direction.normalized * _rayLength;
            }
        }

        private async void Move()
        {
            Vector3 targetPoint = Vector3.zero;

            while (gameObject.activeSelf)
            {
                targetPoint = targetPoint == _firstMovePoint ? _secondMovePoint : _firstMovePoint;
                var startPosition = transform.position;

                float journeyLength = Vector3.Distance(startPosition, targetPoint);
                float elapsedTime = 0;

                Vector3 direction = (targetPoint - startPosition).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;

                while (Vector3.Distance(transform.position, targetPoint) > 1f)
                {
                    float t = elapsedTime / journeyLength;

                    // Обновляем позицию
                    transform.position = Vector3.Lerp(startPosition, targetPoint, t * _speed);
                    elapsedTime += Time.deltaTime;
                    await UniTask.Yield();
                }
            }
        }
    }
}
