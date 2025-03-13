using BouncingBall.Game.Data;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class Mushroom : AbstractEnemy
    {
        public override string Type => EnemyType.Mushroom;


        [Inject]
        public void InitData(GameDataProvider gameDataManager)
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
