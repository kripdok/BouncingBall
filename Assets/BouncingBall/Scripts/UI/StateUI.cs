using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.UI
{
    public abstract class StateUI : MonoBehaviour
    {
        [Inject] public Action OnExit;
    }
}
