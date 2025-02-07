using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public abstract class StateUI : MonoBehaviour
    {
        [Inject] public Action OnExit;
    }
}
