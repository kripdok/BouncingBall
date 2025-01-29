using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;
using BouncingBall.Scripts.Game.GameRoot.UI;
using System;
using System.Collections.Generic;


namespace BouncingBall.Scripts.Game.GameRoot.StateMachine
{
    public class GameStateMachine
    {
        private IState _concreteState;
        private readonly Dictionary<Type, IState> _states;

        public GameStateMachine(SceneLoader sceneLoader, ILoadingWindowController loadingWindowController)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(MainMenuState)] = new MainMenuState(this, sceneLoader, loadingWindowController),
            };
        }

        public void SetState<T>() where T : IState
        {
            if (!_states.TryGetValue(typeof(T), out var newState))
            {
                throw new InvalidOperationException($"State with name {typeof(T)} is not registered!");
            }

            _concreteState?.Exit();
            _concreteState = newState;
            _concreteState.Enter();
        }

    }
}
