using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.Gameplay.LevelObject
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public Transform BallSpawnPoint { get; private set; }

        [SerializeField] private List<Transform> _coinsSpawnPoint;
        [SerializeField] private List<Transform> _enemySpawnPoint;
        [SerializeField] private LevelExit _levelExit;
        //Добавить Тригер окончания уровня 

        public IReadOnlyList<Transform> CoinsSpawnPoint => _coinsSpawnPoint;
        public IReadOnlyList<Transform> EnemySpawnPoint => _enemySpawnPoint;
        public IObservable<Unit> ExitTriggerHit => _levelExit.OnExit;

        private void Awake()
        {
            _levelExit?.gameObject.SetActive(false);
        }

        public void EnableExit()
        {
            Debug.Log("Выход открыт");
            _levelExit?.gameObject.SetActive(true);
        }
    }
}
