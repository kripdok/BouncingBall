using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace BouncingBall.Game.FinalStateMachine
{
    public class GameStateMachine : IStateMachine
    {
        private IState _currentState;
        private readonly Dictionary<string, AbstractGameState> _states;
        private readonly GameStateFactory _stateFactory;

        public GameStateMachine(GameStateFactory stateFactory)
        {
            _stateFactory = stateFactory;

            _states = new Dictionary<string, AbstractGameState>()
            {
                [GameStateTag.Bootstrap] = _stateFactory.Create(GameStateTag.Bootstrap),
                [GameStateTag.MainMenu] = _stateFactory.Create(GameStateTag.MainMenu),
                [GameStateTag.Gameplay] = _stateFactory.Create(GameStateTag.Gameplay),
            };

            foreach (var state in _states.Values)
            {
                state.OnExitObservable.Subscribe(ChangeStateAsync);
            }
        }

        public async void ChangeStateAsync(string stateId)
        {
            if (_states.TryGetValue(stateId, out var newState))
            {
                if (_currentState != null)
                {
                    await _currentState.Exit();
                }

                _currentState = newState;
                _currentState.Enter();
                Debug.Log($"State {stateId} entered");
            }
            else
            {
                throw new NullReferenceException($"There is no game state with id {stateId}!");
            }
        }
    }
}