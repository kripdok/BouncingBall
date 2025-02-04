using Assets.BouncingBall.Scripts.Tests;
using UnityEngine;
using Zenject;

public class TestSceneInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindFactory<Object, TestCreatedGameObject, SumFactory>().FromFactory<PrefabFactory<TestCreatedGameObject>>();
        Debug.Log("Инициализирована сцена");
    }
}
