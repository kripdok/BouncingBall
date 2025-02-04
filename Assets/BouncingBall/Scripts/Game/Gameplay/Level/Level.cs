using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.LevelSystem
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
