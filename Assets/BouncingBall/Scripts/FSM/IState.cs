using Cysharp.Threading.Tasks;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public interface IState
    {
        public string Id { get; }

        public void Enter();
        public UniTask Exit();
    }
}