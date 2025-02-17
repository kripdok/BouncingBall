using Cysharp.Threading.Tasks;

namespace BouncingBall.FinalStateMachine
{
    public interface IState
    {
        public void Enter();
        public UniTask Exit();
    }
}