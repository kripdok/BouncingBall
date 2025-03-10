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

        public IReadOnlyList<Transform> CoinsSpawnPoint => _coinsSpawnPoint;
        public IReadOnlyList<Transform> EnemySpawnPoint => _enemySpawnPoint;
        public IObservable<Unit> OnExitTriggerHit => _levelExit.OnExit;


        public void Reset()
        {
            _levelExit?.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _levelExit?.gameObject.SetActive(false);
        }

        public void EnableExit()
        {
            Debug.Log("The exit is open");
            _levelExit?.gameObject.SetActive(true);
        }
    }
}
