using UnityEngine;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Spikes : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(1);
            }
        }
    }
}
