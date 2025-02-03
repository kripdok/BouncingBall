using Cysharp.Threading.Tasks;

namespace BouncingBall.Scripts.Game.GameRoot.StateMachine.States
{
    public interface IState
    {
        public void Enter();
        public UniTask Exit();
    }
}