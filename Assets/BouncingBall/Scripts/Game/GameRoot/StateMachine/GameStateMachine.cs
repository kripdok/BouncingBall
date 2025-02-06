using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using System;
using System.Collections.Generic;


namespace BouncingBall.Scripts.Game.GameRoot.StateMachine
{
    public class GameStateMachine
    {
        private IState _concreteState;
        private readonly Dictionary<Type, IState> _states;

        public GameStateMachine(SceneLoader sceneLoader, IAttachStateUI attachStateUI, ILoadingWindowController loadingWindowController, IInputInteractivityChanger manageInputState, IPrefabLoadStrategy prefabLoadStrategy, LevelLoaderMediator levelLoaderMediator, StateUIFactory stateUIFactory, GameInformation gameInformation)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(MainMenuState)] = new MainMenuState(this, sceneLoader, loadingWindowController, attachStateUI, prefabLoadStrategy, stateUIFactory, levelLoaderMediator),
                [typeof(GameplayState)] = new GameplayState(this, manageInputState, loadingWindowController, attachStateUI, prefabLoadStrategy, levelLoaderMediator, stateUIFactory),
            };
        }

        public async void SetState<T>() where T : IState
        {
            if (!_states.TryGetValue(typeof(T), out var newState))
            {
                throw new InvalidOperationException($"State with name {typeof(T)} is not registered!");
            }

            if (_concreteState != null)
            {
                await _concreteState.Exit();
            }

            _concreteState = newState;
            _concreteState.Enter();
        }

    }
}