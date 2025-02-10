﻿using BouncingBall.FinalStateMachine;
using BouncingBall.Game.FinalStateMachine.States;

namespace BouncingBall.Game.Gameplay.Root
{
    public class GameplayBootstrap
    {
        public GameplayBootstrap(IStateMachine gameStateMachine)
        {
            gameStateMachine.SetState(GameStateNames.MainMenu);
        }
    }
}
