using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public Transform BallSpawnPoint { get; private set; }

        private LevelModel _model;

        public void Init(LevelModel model)
        {
            _model = model;
            _model.Ball.transform.position = BallSpawnPoint.position;
        }

    }
}
