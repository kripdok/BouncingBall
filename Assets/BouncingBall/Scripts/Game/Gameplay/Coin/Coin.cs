using BouncingBall.Game.Gameplay.BallObject;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    [RequireComponent(typeof(Rigidbody))]
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Collider _collider;


        private Rigidbody _rigidbody;
        private CoinData _data;

        public void SetData(CoinData data)
        {
            _data = data;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject);
            if(other.TryGetComponent<Ball>(out var ball))
            {
                _data.SendReword();
                PlayDisappearingAnimation();
            }
            
        }

        private void PlayDisappearingAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
