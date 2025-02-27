using BouncingBall.Game.Gameplay.Entities.BallEntity;
using System;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    [RequireComponent(typeof(Rigidbody))]
    public class Coin : MonoBehaviour
    {
        [Inject] private CoinsPool _pool;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private CoinData _data;

        public IObservable<int> Reword => _data.Reword;

        public void SetData(CoinData data)
        {
            _data = data;
        }
        public void Reset()
        {
            gameObject.SetActive(true);
        }

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
                _data.SendReword();
                PlayDisappearingAnimation();
            }
        }

        private void OnDisable()
        {
            _pool.Despawn(this);
        }

        private void PlayDisappearingAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
