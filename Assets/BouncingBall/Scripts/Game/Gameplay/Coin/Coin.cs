using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    [RequireComponent(typeof(Rigidbody))]
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Collider _collider;


        private Rigidbody _rigidbody;
        [Inject] private CoinData _data;

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
            _data.SendReword();
            //TODO - Происходит вызов подбора монетки + анимация
            PlayDisappearingAnimation();
        }

        private void PlayDisappearingAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
