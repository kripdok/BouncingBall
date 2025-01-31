using Zenject;
using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBall();
        }

        private void BindBall()
        {
            var prefab = Resources.Load<Ball>("Prefabs/Gameplay/Ball");
            var ball = Instantiate(prefab);
            Container.Bind<Ball>().FromInstance(ball).AsCached();
        }
    }
}
