using BouncingBall.Game.Data;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Cactus : AbstractEnemy
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _deathAnimationDurationduration = 0.5f;
        [SerializeField] private float _rayLength = 10f;
        [SerializeField] private ParticleSystem _deathEffect;

        private Vector3 _defoltScale;
        private Vector3 _firstMovePoint;
        private Vector3 _secondMovePoint;
        private bool _isWoork;

        public override string Type => EnemyType.Cactus;

        public override void Reset()
        {
            base.Reset();
            transform.localScale = _defoltScale;
            Collider.enabled = true;
            _firstMovePoint = GetRaycastPoint(Vector3.left);
            _secondMovePoint = GetRaycastPoint(Vector3.right);

            Move();
        }

        [Inject]
        public void InitData(GameDataManager gameDataManager)
        {
            HealthSystem = new(gameDataManager.GameData.CactusData.MaxHealthAmount);
            _speed = gameDataManager.GameData.CactusData.MaxMoveSpeed;
        }

        protected override void Awake()
        {
            base.Awake();
            _defoltScale = transform.localScale;
        }

        private void OnDisable()
        {
            _isWoork = false;
        }

        protected override bool IsTakeDamage(Collision collision)
        {
            Vector3 enemyNormal = collision.contacts[0].normal;

            Vector3 localNormal = transform.InverseTransformDirection(enemyNormal);

            var angle = Mathf.Atan2(localNormal.x, localNormal.z) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            bool one = (angle >= 350 || angle <= 5);
            bool two = (angle >= 170 && angle <= 190);

            return one || two;
        }

        protected override void TakeDamage()
        {
            PlayDisappearingAnimation();
        }

        private async void PlayDisappearingAnimation()
        {
            Collider.enabled = false;

            Vector3 initialPosition = transform.position;
            Vector3 initialScale = _defoltScale;

            float elapsedTime = 0f;
            _deathEffect.Play();

            while (elapsedTime < _deathAnimationDurationduration)
            {
                float t = elapsedTime / _deathAnimationDurationduration;
                transform.Rotate(Vector3.up, 360 * Time.deltaTime / _deathAnimationDurationduration);
                transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            while (_deathEffect.isPlaying)
            {
                await Task.Yield();
            }

            Pool.Remove(this);
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
            await UniTask.Yield();
            _isWoork = true;

            while (_isWoork)
            {
                targetPoint = targetPoint == _firstMovePoint ? _secondMovePoint : _firstMovePoint;
                var startPosition = transform.position;

                float journeyLength = Vector3.Distance(startPosition, targetPoint);
                float elapsedTime = 0;

                Vector3 direction = (targetPoint - startPosition).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
                var position = transform.position;

                while (Vector3.Distance(position, targetPoint) > 1f && _isWoork)
                {
                    float t = elapsedTime / journeyLength;

                    transform.position = Vector3.Lerp(startPosition, targetPoint, t * _speed);
                    elapsedTime += Time.deltaTime;
                    position = transform.position;  
                    await UniTask.Yield();
                }
            }
        }
    }
}
