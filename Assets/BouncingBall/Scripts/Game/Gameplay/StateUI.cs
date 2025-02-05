using BouncingBall.Scripts.Game.Gameplay.Root;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public abstract class StateUI : MonoBehaviour
    {
        [Inject] public GameInformation GameInformation;
        [Inject] public LevelLoader levelLoader;

    }
}
