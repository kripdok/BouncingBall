using UnityEngine;

namespace Assets.BouncingBall.Scripts.Game.Gameplay.Enemy
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
