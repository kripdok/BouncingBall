using Zenject;
using UnityEngine;
using BouncingBall.Scripts.InputSystem.Controller;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Game.Gameplay.BallSystem;

namespace BouncingBall.Scripts.Game.Gameplay.Root
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {

           
            Container.Bind<LevelLoader>().AsCached().NonLazy();
            //BindPointHolder();
            //BindBall();

        }

        private void BindBall()
        {
            Debug.Log("1");
            var prefab = Resources.Load<Ball>("Prefabs/Gameplay/Ball");
            var ball = Instantiate(prefab);
            ball.Сonstructor(Container.Resolve<InputController>());
            
            Container.Bind<Ball>().FromInstance(ball).AsCached();
            Debug.Log("2");
        }

        private void BindPointHolder()
        {
            var prefab = Resources.Load<BallDirectionSign>("Prefabs/Gameplay/PointHolder");
            var ballDirectionSign = Instantiate(prefab,Container.Resolve<Ball>().transform);
            ballDirectionSign.Сonstructor(Container.Resolve<InputController>());
            Container.Bind<BallDirectionSign>().FromInstance(ballDirectionSign).AsCached();
        }
    }
}
