using BouncingBall.Game.Gameplay.BallObject;
using System;
using UniRx;
using UnityEngine;

namespace BouncingBall.Game.Gameplay.LevelObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class LevelExit : MonoBehaviour
    {
        public IObservable<Unit> OnExit => _onExit;
        private ReactiveCommand _onExit = new();
        private Rigidbody _rigidbody;

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
