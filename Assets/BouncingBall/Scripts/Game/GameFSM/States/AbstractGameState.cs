using BouncingBall.FinalStateMachine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public abstract class AbstractGameState : IState
    {
        public ISubject<string> IOnExit => OnExit;

        public readonly string Id;
        protected readonly Subject<string> OnExit;        

        public AbstractGameState(string id)
        {
            Id = id;
            OnExit = new Subject<string>();
        }

        public abstract void Enter();

        public abstract UniTask Exit();
    }
}
