
using Assets.BouncingBall.Scripts.Tests;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Tests
{
    public class SumCreatedObject:MonoBehaviour
    {
        [SerializeField] private TestCreatedGameObject objectInstaller;

        [Inject]
        public void Container(SumFactory factory)
        {
            factory.Create(objectInstaller);
            Debug.Log("Контейнер объекта");
        }

    }
}
