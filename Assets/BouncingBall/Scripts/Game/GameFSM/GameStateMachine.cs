using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;
using BouncingBall.InputSystem.Controller;
using BouncingBall.PrefabLoader;
using BouncingBall.UI;
using BouncingBall.UI.Root;
using BouncingBall.Utilities;
using System;
using System.Collections.Generic;


namespace BouncingBall.Game.FinalStateMachine
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