using UnityEngine;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        protected Collider Collider;
        //TODO - добавить пулл объектов

        public virtual void Reset()
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.identity;
        }

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnDisable()
        {
            //_pool.Despawn(this);
        }

        protected abstract bool IsCollisionPerpendicular(Collision collision);
    }
}
