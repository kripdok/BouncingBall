using Cysharp.Threading.Tasks;

namespace BouncingBall.UI.Root
{
    public interface ILoadingWindowController
    {
        public UniTask HideLoadingScreen();
        public UniTask ShowLoadingScreen();
    }
}
