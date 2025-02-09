using Cysharp.Threading.Tasks;

namespace BouncingBall.UI.Root
{
    public interface ILoadingWindowController
    {
        public UniTask HideLoadingWindow();
        public UniTask ShowLoadingWindow();
    }
}
