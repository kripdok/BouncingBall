using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [Inject] protected EnemyPool Pool;

        protected Collider Collider;

        public abstract EnemyType Type { get; }

        public virtual void Reset()
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.identity;
        }

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        protected abstract bool IsCollisionPerpendicular(Collision collision);
    }
}
