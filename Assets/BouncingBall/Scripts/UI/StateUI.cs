using UniRx;
using UnityEngine;

namespace BouncingBall.UI
{
    public abstract class StateUI : MonoBehaviour
    {
        public readonly Subject<string> ExitRequested = new();
    }
}
