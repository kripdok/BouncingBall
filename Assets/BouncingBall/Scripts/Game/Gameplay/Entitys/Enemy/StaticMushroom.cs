using UnityEngine;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class StaticMushroom : AbstractEnemy
    {
        public override EnemyType Type => EnemyType.Mushroom;

        protected override bool IsTakeDamage(Collision collision)
        {
            return false;
        }

        protected override void TakeDamage()
        {
            return;
        }
    }
}
