using BouncingBall.Game.Data;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Mushroom : AbstractEnemy
    {
        public override EnemyType Type => EnemyType.Mushroom;


        [Inject]
        public void InitData(GameDataManager gameDataManager)
        {
            HealthSystem = new(gameDataManager.GameData.MushroomData.MaxHealthAmount);
        }

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
