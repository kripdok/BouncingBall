using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class BootstrapState : AbstractGameState
    {
        [Inject] private readonly SceneLoader _sceneLoader;

        public override async void Enter()
        {
            await _sceneLoader.LoadSceneAsync(SceneTag.PreLoader);
            await _sceneLoader.LoadSceneAsync(SceneTag.Gameplay);
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
