using UnityEngine;
    using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.BallEntity
{
    public class BallCollisionEffectPool : MemoryPool<Vector3, Quaternion, BallCollisionEffect>
    {
        protected override void Reinitialize(Vector3 position, Quaternion rotation, BallCollisionEffect item)
        {
            item.Reset(position, rotation);
        }
    }
}
