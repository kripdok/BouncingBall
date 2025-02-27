using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.CustomPhysics;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.Utilities.Reset;
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
        [SerializeField] private float _speedMultiplier = 200;
        [SerializeField] private Transform _body;

        [Inject] private IInputManager _inputManager;

        private CompositeDisposable _inputDeviceDisposable;
        private CustomRigidbody _rigidbody;
        private BallData _model;
        private Vector3 _moveDirection;
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
            _rigidbody = GetComponent<CustomRigidbody>();
            Subscribe();
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

            _inputManager.ZScale.Subscribe(SetSpeed).AddTo(_inputDeviceDisposable);
            _inputManager.RotationAmount.Subscribe(direction => _moveDirection = direction).AddTo(_inputDeviceDisposable);
            _inputManager.IsDirectionSet.Skip(1).Subscribe(TryAddForce).AddTo(_inputDeviceDisposable);
        }

        private void SetSpeed(float speed)
        {
            _speed = Math.Clamp(speed, 0, _model.MaxSpeed);
        }

        private void TryAddForce(bool flag)
        {
            if (flag == false)
            {
                _rigidbody.AddForce(_moveDirection * _speed * _speedMultiplier);
            }
        }
    }
}
