using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;
using System;
using System.Collections.Generic;
using UniRx;


namespace BouncingBall.Game.FinalStateMachine
{
    public class GameStateMachine : IStateMachine
    {
        private IState _concreteState;
        private readonly Dictionary<string, AbstractGameState> _states;
        private readonly GameStateFactory _stateFactory;

        public GameStateMachine(GameStateFactory stateFactory)
        {
            _stateFactory = stateFactory;

            _states = new Dictionary<string, AbstractGameState>()
            {
                [GameStateNames.Bootstrap] = _stateFactory.Create(GameStateNames.Bootstrap),
                [GameStateNames.MainMenu] = _stateFactory.Create(GameStateNames.MainMenu),
                [GameStateNames.Gameplay] = _stateFactory.Create(GameStateNames.Gameplay),
            };

            foreach(var state in _states.Values)
            {
                state.IOnExit.Subscribe(SetState);
            }
        }

        public async void SetState(string id)
        {
            if (_states.TryGetValue(id, out var newState))
            {
                if (_concreteState != null)
                {
                    await _concreteState.Exit();
                }

                _concreteState = newState;
                _concreteState.Enter();
            }
            else
            {
                throw new NullReferenceException($"There is no game state with id {id}!");
            }
        }
    }
}