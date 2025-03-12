using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.BallEntity
{
    public class BallCollisionEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;

        [Inject] private BallCollisionEffectPool _pool;

        private bool _isWork;

        public void Reset(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            PlayEffectAndDespawn();
        }
        private void Awake()
        {
            _isWork = true;
        }

        private void OnDestroy()
        {
            _isWork = false;
        }

        private async void PlayEffectAndDespawn()
        {
            _effect.Play();

            while (_isWork && _effect.isPlaying)
            {
                await UniTask.Yield();
            }

            if (!_isWork)
            {
                return;
            }

            _effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _pool.Despawn(this);
        }
    }
}
