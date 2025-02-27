using BouncingBall.Game.Gameplay.Entities.BallEntity;
using System;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.Gameplay.LevelObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class LevelExit : MonoBehaviour
    {
        private ReactiveCommand _onExit = new();
        private Rigidbody _rigidbody;

        public IObservable<Unit> OnExit => _onExit;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                _onExit.Execute();
            }
        }
    }
}
