using Assets.BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Я зарегистрировался");
        
    }
}
