using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.LevelSystem
{
    public class LevelModel
    {
        [Inject] public readonly Ball Ball;
    }
}
