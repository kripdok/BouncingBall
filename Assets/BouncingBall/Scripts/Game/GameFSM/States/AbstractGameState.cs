using BouncingBall.FinalStateMachine;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

namespace BouncingBall.Game.FinalStateMachine.States
{
    public abstract class AbstractGameState : IState
    {
        public IObservable<string> OnExitObservable => OnExit;

        public readonly string StateId;
        protected readonly Subject<string> OnExit;

        public AbstractGameState(string stateId)
        {
            StateId = stateId;
            OnExit = new Subject<string>();
        }

        public abstract void Enter();

        public abstract UniTask Exit();
    }
}
