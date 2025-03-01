using BouncingBall.CustomPhysics;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.InputSystem;
using BouncingBall.Utilities.Reset;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BouncingBall.Game.Gameplay.Entities.BallEntity
{
    [RequireComponent(typeof(CustomRigidbody))]
    public class Ball : MonoBehaviour, IPointerDownHandler, IResettable, IDamageable
    {
        [SerializeField] private Transform _body;
        [SerializeField, Range(0, 1)] private float _compressionDuration;
        [SerializeField, Range(0, 10)] private float _maximumCompression;

        [Inject] private IInputManager _inputManager;

        private CompositeDisposable _inputDeviceDisposable;
        private CustomRigidbody _rigidbody;
        private BallData _model;
        private Vector3 _moveDirection;
        private Vector3 _originalScale;
        private float _speed;

        [Inject]
        private void Constructor(GameDataManager GameDataManager, ResetManager resetManager)
        {
            resetManager.Add(this);
            _model = GameDataManager.GameData.BallModel;
        }

        public void Reset()
        {
            _rigidbody.Reset();
            _body.rotation = Quaternion.identity;
            _model.ResetHealth();
        }

        private void Awake()
        {
            _originalScale = transform.localScale;
            _rigidbody = GetComponent<CustomRigidbody>();
            Subscribe();
        }

        private async void OnCollisionEnter(Collision collision)
        {
            Vector3 normal = collision.GetContact(0).normal;
            var newVelocity = Vector3.Reflect(_rigidbody._velocityForce, normal);
            Debug.Log("Игрок"+newVelocity);
            await CompressScale(newVelocity);
            await UnclenchScale();

        }

     

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputManager.EnableControllable();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                Debug.LogError($"Object can't take negative damage! Damage: {damage}");
                return;
            }

            _model.AddDamage(damage);
        }

        private void Subscribe()
        {
            _inputManager.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
            Observable.EveryUpdate().Subscribe(_ => _model.Position.Value = transform.position).AddTo(this);
            Observable.EveryUpdate().Subscribe(_ => _model.Direction.Value = _rigidbody.TestVelocity).AddTo(this);

            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputManager.ZScale?.Subscribe(SetSpeed).AddTo(_inputDeviceDisposable);
            _inputManager.RotationAmount?.Subscribe(direction => _moveDirection = direction).AddTo(_inputDeviceDisposable);
            _inputManager.IsDirectionSet?.Skip(1).Subscribe(TryAddForce).AddTo(_inputDeviceDisposable);
        }

        private void SetSpeed(float speed)
        {
            _speed = Math.Clamp(speed, 0, _model.MaxSpeed);
        }

        private void TryAddForce(bool flag)
        {
            if (flag == false)
            {
                _rigidbody.AddForce(_moveDirection * _speed);
            }
        }

        private async UniTask CompressScale(Vector3 newVelocity)
        {
            var powerCompression = GetVelocityForcePowerCompression();
            var compressionScale = GetCinoressScale(powerCompression);

            float elapsedTime = 0f;

            while (elapsedTime < _compressionDuration)
            {
                float lerpT = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, compressionScale, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private async UniTask UnclenchScale()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _compressionDuration)
            {
                float lerpT = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, lerpT);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            transform.localScale = Vector3.one;
        }

        private float GetVelocityForcePowerCompression()
        {
            var number = Vector3.Distance(Vector3.zero, _rigidbody._velocityForce);
            return Mathf.Clamp(number, 0, _maximumCompression);
        }

        private Vector3 GetCinoressScale(float powerCompression)
        {
            var scale = transform.localScale;
            scale.z = scale.z - (powerCompression / 10);
            return scale;
        }
    }
}
