using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.BallEntity
{
    public class BallCollisionEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;

        [Inject] private BallCollisionEffectPool _pool;

        public void Reset(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            PlayEffectAndDespawn();
        }

        private async void PlayEffectAndDespawn()
        {
            _effect.Play();

            while (_effect.gameObject != null & _effect.isPlaying)
            {
                await UniTask.Yield();
            }

            _effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _pool.Despawn(this);
        }
    }
}
