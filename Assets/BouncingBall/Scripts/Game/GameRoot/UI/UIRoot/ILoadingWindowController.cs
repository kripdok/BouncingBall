using Cysharp.Threading.Tasks;

namespace BouncingBall.Scripts.Game.GameRoot.UI
{
    public interface ILoadingWindowController
    {
        public UniTask HideLoadingWindow();
        public UniTask ShowLoadingWindow();
    }
}
