using BouncingBall.Game.GameRoot.Constants;
using BouncingBall.Utilities;
using Cysharp.Threading.Tasks;
using Zenject;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public class BootstrapState : AbstractGameState
    {
        [Inject] private readonly SceneLoader _sceneLoader;

        public BootstrapState() : base(GameStateTag.Bootstrap) { }

        public override async void Enter()
        {
            await _sceneLoader.LoadScene(SceneTag.PreLoader);
            await _sceneLoader.LoadScene(SceneTag.Gameplay);
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
