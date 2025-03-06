﻿using BouncingBall.CustomPhysics;
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
        [SerializeField, Range(0, 1)] private float _compressionDuration = 0.2f;
        [SerializeField, Range(0, 10)] private float _maximumCompression = 5f;
        [SerializeField] private Material _material;
        [SerializeField] private Color _compressionColor = Color.red;

        [Inject] private IInputManager _inputManager;
        [Inject] private BallCollisionEffectPool _collisionEffectPool;

        private CompositeDisposable _inputDisposables;
        private CustomRigidbody _rigidbody;
        private BallData _ballData;
        private Vector3 _moveDirection;
        private Vector3 _originalScale;
        private float _currentSpeed;
        private readonly Color _defaultColor = Color.white;

        [Inject]
        private void Construct(GameDataManager gameDataManager, ResetManager resetManager)
        {
            resetManager.Add(this);
            _ballData = gameDataManager.GameData.BallData;
        }

        public void Reset()
        {
            _rigidbody.Reset();
            _body.rotation = Quaternion.identity;
            _ballData.HealthSystem.Reset();
        }

        private void Awake()
        {
            _originalScale = transform.localScale;
            _rigidbody = GetComponent<CustomRigidbody>();
            SubscribeToInputEvents();
        }

        private async void OnCollisionEnter(Collision collision)
        {
            Vector3 collisionNormal = collision.GetContact(0).normal;
            Vector3 reflectedVelocity = Vector3.Reflect(_rigidbody._velocityForce, collisionNormal);

            _collisionEffectPool.Spawn(collision.GetContact(0).point, Quaternion.LookRotation(collisionNormal));
            await CompressBall(reflectedVelocity);
            await RestoreBallScale();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputManager.EnableControllable();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                Debug.LogError($"Damage must be positive! Received: {damage}");
                return;
            }

            _ballData.HealthSystem.TakeDamage(damage);
        }

        private void SubscribeToInputEvents()
        {
            _inputManager.InputChange
                .Subscribe(_ => SubscribeToInputDevice())
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ => UpdateBallData())
                .AddTo(this);

            SubscribeToInputDevice();
        }

        private void SubscribeToInputDevice()
        {
            _inputDisposables?.Dispose();
            _inputDisposables = new CompositeDisposable();

            _inputManager.ZScale?
                .Subscribe(SetCurrentSpeed)
                .AddTo(_inputDisposables);

            _inputManager.RotationAmount?
                .Subscribe(direction => _moveDirection = direction)
                .AddTo(_inputDisposables);

            _inputManager.IsDirectionSet?
                .Skip(1)
                .Subscribe(ApplyForceIfDirectionSet)
                .AddTo(_inputDisposables);
        }

        private void SetCurrentSpeed(float speed)
        {
            _currentSpeed = Math.Clamp(speed, 0, _ballData.MaxSpeed);
        }

        private void ApplyForceIfDirectionSet(bool isDirectionSet)
        {
            if (!isDirectionSet)
            {
                _rigidbody.AddForce(_moveDirection * _currentSpeed);
            }
        }

        private void UpdateBallData()
        {
            _ballData.Position.Value = transform.position;
            _ballData.Direction.Value = _rigidbody.TestVelocity;
        }

        private async UniTask CompressBall(Vector3 newVelocity)
        {
            float compressionPower = CalculateCompressionPower();
            Vector3 targetScale = CalculateCompressionScale(compressionPower);
            Color targetColor = CalculateCompressionColor(compressionPower);

            float elapsedTime = 0f;

            while (elapsedTime < _compressionDuration)
            {
                float lerpValue = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpValue);
                _material.color = Color.Lerp(_defaultColor, targetColor, lerpValue);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }

        private async UniTask RestoreBallScale()
        {
            float elapsedTime = 0f;
            Color initialColor = _material.color;

            while (elapsedTime < _compressionDuration)
            {
                float lerpValue = elapsedTime / _compressionDuration;
                transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, lerpValue);
                _material.color = Color.Lerp(initialColor, _defaultColor, lerpValue);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            _material.color = _defaultColor;
            transform.localScale = _originalScale;
        }

        private float CalculateCompressionPower()
        {
            float velocityMagnitude = Vector3.Distance(Vector3.zero, _rigidbody._velocityForce);
            return Mathf.Clamp(velocityMagnitude, 0, _maximumCompression);
        }

        private Vector3 CalculateCompressionScale(float compressionPower)
        {
            Vector3 scale = transform.localScale;
            scale.z -= compressionPower / 10f;
            return scale;
        }

        private Color CalculateCompressionColor(float compressionPower)
        {
            float t = Mathf.Clamp01(compressionPower / _maximumCompression);
            return Color.Lerp(_defaultColor, _compressionColor, t);
        }
    }
}
