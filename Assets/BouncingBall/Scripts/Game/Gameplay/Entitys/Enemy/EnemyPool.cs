using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class EnemyPool : IFactory<EnemyType, AbstractEnemy>
    {
        private const string SpikesPrefabPath = "Prefabs/Gameplay/Enemy";

        private DiContainer _container;
        private List<AbstractEnemy> _activeEnemys;

        private Transform _parent;

        public EnemyPool(DiContainer container)
        {
            _activeEnemys = new();
            _container = container;
            _parent = new GameObject("Enemies").transform;
        }

        public AbstractEnemy Create(EnemyType param)
        {
            var obj = _activeEnemys.FirstOrDefault(enemy => param == enemy.Type && !enemy.gameObject.activeSelf);

            if (obj == null)
            {
                switch (param)
                {
                    case EnemyType.Cactus:
                        obj = CreateObject(SpikesPrefabPath);
                        break;
                    default:
                        throw new ArgumentException($"Prefab of type {param} is not registered in pool");
                }
            }

            obj.Reset();
            return obj;
        }


        public void Despawn(AbstractEnemy enemy)
        {
            if (_activeEnemys.Contains(enemy))
            {
                enemy.gameObject.SetActive(false);
            }
            else
            {
                throw new ArgumentException($"Object {enemy} is not registered in pool");
            }
        }

        private AbstractEnemy CreateObject(string path)
        {
            var obj = _container.InstantiatePrefabResource(SpikesPrefabPath).GetComponent<AbstractEnemy>();
            obj.transform.parent = _parent;
            return obj;
        }
    }
}
