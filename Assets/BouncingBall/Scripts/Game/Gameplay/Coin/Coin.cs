using BouncingBall.Game.Gameplay.Entities.BallEntity;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    [RequireComponent(typeof(Rigidbody))]
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Transform _body;
        [Header("Effects")]
        [SerializeField] private ParticleSystem _pulsationEffect;
        [SerializeField] private ParticleSystem _explosionEffect;

        [Inject] private CoinsPool _pool;

        private Rigidbody _rigidbody;
        private CoinData _data;
        private Vector3 _defoltScale;
        private bool _isColliderDetected;
        private Vector3 _sumVector = new Vector3(0, 1, 0);

        public IObservable<int> Reword => _data.Reword;


        public void Reset()
        {
            gameObject.SetActive(true);
            _body.rotation = Quaternion.identity;
            _body.localScale = _defoltScale;
            _isColliderDetected = true;
            _pulsationEffect.Play();
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            var collider = GetComponent<Collider>();
            collider.isTrigger = true;

            _defoltScale = _body.localScale;
            _isColliderDetected = true ;
        }

        private void Update()
        {
            _body.transform.localRotation = Quaternion.Euler(_sumVector) * Quaternion.Euler(_body.transform.localRotation.eulerAngles);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Ball>(out var ball)&& _isColliderDetected)
            {
                _data.SendReword();
                _isColliderDetected = false;
                PlayDisappearingAnimation();
            }
        }

        private void OnDisable()
        {
            _pool.Despawn(this);
        }

        public void SetData(CoinData data)
        {
            _data = data;
        }

        private async void PlayDisappearingAnimation()
        {
            _pulsationEffect.Stop();
            _explosionEffect.Play();

            Vector3 initialPosition = transform.position;
            Vector3 initialScale = _defoltScale;

            float elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                float t = elapsedTime / _duration;

                _body.Rotate(Vector3.up, 360 * Time.deltaTime / _duration);
                _body.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            gameObject.SetActive(false);
        }
    }
}
