using BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.GameRoot.StateMachine.States;
using BouncingBall.Scripts.Game.GameRoot.UI;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Utilities.PrefabLoad;
using System;
using System.Collections.Generic;


namespace BouncingBall.Scripts.Game.GameRoot.StateMachine
{
    public class GameStateMachine : IStateMachine
    {
        private IState _concreteState;
        private readonly Dictionary<string, IState> _states;

        public GameStateMachine(SceneLoader sceneLoader, IAttachStateUI attachStateUI, ILoadingWindowController loadingWindowController, IInputInteractivityChanger manageInputState, IPrefabLoadStrategy prefabLoadStrategy, LevelLoaderMediator levelLoaderMediator, StateUIFactory stateUIFactory)
        {
            _states = new Dictionary<string, IState>()
            {
                [GameStateNames.Bootstrap] = new BootstrapState(this, sceneLoader),
                [GameStateNames.MainMenu] = new MainMenuState(this, loadingWindowController, attachStateUI, prefabLoadStrategy, stateUIFactory, levelLoaderMediator),
                [GameStateNames.Gameplay] = new GameplayState(this, manageInputState, loadingWindowController, attachStateUI, prefabLoadStrategy, levelLoaderMediator, stateUIFactory),
            };
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
                throw new NullReferenceException("");
            }
        }
    }
}