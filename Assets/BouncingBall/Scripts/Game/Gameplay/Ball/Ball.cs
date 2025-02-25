﻿using Assets.BouncingBall.Scripts.Game.Gameplay;
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



namespace BouncingBall.Game.Gameplay.BallObject
{
    [RequireComponent(typeof(CustomRigidbody))]
    public class Ball : MonoBehaviour, IPointerDownHandler, IResettable, IDamageable
    {
        [SerializeField] private float _speedMultiplier = 200;
        [SerializeField] private Transform _body;

        private CustomRigidbody _rigidbody;
        private BallData _model;

        private IInputManager _inputManager;
        private CompositeDisposable _inputDeviceDisposable;
        private Vector3 _moveDirection;
        private float _speed;


        [Inject]
        public void Constructor(GameDataManager GameDataManager, IInputManager inputManager, ResetManager resetManager)
        {
            resetManager.Add(this);
            _rigidbody = GetComponent<CustomRigidbody>();
            _model = GameDataManager.GameData.BallModel;
            _inputManager = inputManager;

            _inputManager.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
            Observable.EveryUpdate().Subscribe(_ => _model.Position.Value = transform.position).AddTo(this);
            Observable.EveryUpdate().Subscribe(_ => _model.Direction.Value = _rigidbody.TestVelocity).AddTo(this);
        
            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            _inputDeviceDisposable?.Dispose();
            _inputDeviceDisposable = new();

            _inputManager.ZScale.Subscribe(SetSpeer).AddTo(_inputDeviceDisposable);
            _inputManager.RotationAmount.Subscribe(direction => _moveDirection = direction).AddTo(_inputDeviceDisposable);
            _inputManager.IsDirectionSet.Skip(1).Subscribe(Punch2).AddTo(_inputDeviceDisposable);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _inputManager.EnableControllable();
        }

        private void SetSpeer(float speed)
        {
            _speed = Math.Clamp(speed, 0, _model.MaxSpeed);
        }

        private void Punch2(bool flag)
        {
            if (flag == false)
            {
                _rigidbody.AddForce(_moveDirection * _speed * _speedMultiplier);
            }
        }

        public void Reset()
        {
            _rigidbody.Reset();
            _body.rotation = Quaternion.identity;
            _model.ResetHealth();
        }

        public void TakeDamage(int damage)
        {
            if(damage <= 0)
            {
                Debug.LogError($"Object can't take negative damage! Damage: {damage}");
                return;
            }

            _model.AddDamage(damage);
        }
    }
}
