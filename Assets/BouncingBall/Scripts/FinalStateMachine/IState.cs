using Cysharp.Threading.Tasks;

namespace BouncingBall.FinalStateMachine
{
    public interface IState
    {
        public string Id { get; }

        public void Enter();
        public UniTask Exit();
    }
}