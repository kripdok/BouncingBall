using BouncingBall.Scripts.Game.Gameplay.Root;
using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public abstract class StateUI : MonoBehaviour
    {
        [Inject] public GameInformation GameInformation;
        [Inject] public Action OnExit;
    }
}
