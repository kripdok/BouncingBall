using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public Transform BallSpawnPoint { get; private set; }

        [Inject]
        public void Сonstructor(Ball ball)
        {
            ball.transform.position = BallSpawnPoint.position;
        }

    }
}
