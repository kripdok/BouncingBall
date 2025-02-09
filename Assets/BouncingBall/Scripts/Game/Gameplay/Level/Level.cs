using BouncingBall.Game.Gameplay.BallObject;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.LevelObject
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public Transform BallSpawnPoint { get; private set; }

        private Ball _ball;

        [Inject]
        public void Construct(Ball ball)
        {
            _ball = ball;
            _ball.transform.position = BallSpawnPoint.position;
        }

    }
}
