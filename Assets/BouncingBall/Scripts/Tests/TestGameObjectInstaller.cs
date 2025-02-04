using UnityEngine;
using Zenject;

public class TestGameObjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log( "Инсталлер игрового объекта");
    }
}