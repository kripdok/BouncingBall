using BouncingBall.Utilities.HealthSystems;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [Inject] protected EnemyPool Pool;

        protected HealthSystem HealthSystem;
        protected Collider Collider;

        private int _damage = 1;

        public abstract EnemyType Type { get; }

        public virtual void Reset()
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.identity;
            HealthSystem.Reset();
        }

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (IsTakeDamage(collision))
                {
                    TakeDamage();
                }
                else
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        protected abstract bool IsTakeDamage(Collision collision);
        protected abstract void TakeDamage();
    }
}
