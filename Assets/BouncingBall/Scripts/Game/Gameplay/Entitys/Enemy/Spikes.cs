using UnityEngine;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Spikes : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                Vector3 enemyNormal = collision.contacts[0].normal;

                Vector3 localNormal = transform.InverseTransformDirection(enemyNormal);

                var angle = Mathf.Atan2(localNormal.x, localNormal.z) * Mathf.Rad2Deg;
                angle = (angle + 360) % 360;

                bool one = (angle >= 350 || angle <= 5);
                bool two = (angle >= 170 && angle <= 190);

                Debug.Log("Противник считает " + angle);

                if (one || two)
                {
                   gameObject.SetActive(false);
                }
                else
                {
                    damageable.TakeDamage(1);
                }
            }
        }
    }
}
