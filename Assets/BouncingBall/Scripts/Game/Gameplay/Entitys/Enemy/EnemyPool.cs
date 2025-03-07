using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.EnemyEntity
{
    public class EnemyPool : IFactory<Vector3, string, AbstractEnemy>
    {
        private const string SpikesPrefabPath = "Prefabs/Gameplay/Enemy_Cactus";
        private const string MushroomPrefabPath = "Prefabs/Gameplay/Enemy_Mushroom";

        private DiContainer _container;
        private List<AbstractEnemy> _activeEnemys;

        private Transform _parent;

        public EnemyPool(DiContainer container)
        {
            _activeEnemys = new();
            _container = container;
            _parent = new GameObject("Enemies").transform;
        }

        public AbstractEnemy Create(Vector3 param1, string param2)
        {
            var obj = _activeEnemys.FirstOrDefault(enemy => param2 == enemy.Type && !enemy.gameObject.activeSelf);

            if (obj == null)
            {
                switch (param2)
                {
                    case EnemyType.Cactus:
                        obj = CreateObject(SpikesPrefabPath);
                        break;
                    case EnemyType.Mushroom:
                        obj = CreateObject(MushroomPrefabPath);
                        break;
                    default:
                        throw new ArgumentException($"Prefab of type {param2} is not registered in pool");
                }

                _activeEnemys.Add(obj);
            }

            obj.transform.position = param1;
            obj.Reset();
            return obj;
        }


        public void Remove(AbstractEnemy enemy)
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
            var obj = _container.InstantiatePrefabResource(path).GetComponent<AbstractEnemy>();
            obj.transform.parent = _parent;
            return obj;
        }
    }
}
