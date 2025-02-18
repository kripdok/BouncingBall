using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.CustomPhysics;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;



namespace BouncingBall.Game.Gameplay.BallObject
{
    [RequireComponent(typeof(CustomRigidbody))]
    public class Ball : MonoBehaviour, IPointerDownHandler
    {
        private CustomRigidbody _rigidbody;
        private BallData _model;

        private IInputManager _inputManager;
        private CompositeDisposable _inputDeviceDisposable;
        private Vector3 _moveDirection;
        private float _speed;

        [Inject]
        public void Constructor(GameDataManager GameDataManager, IInputManager inputManager)
        {
            _model = GameDataManager.GameData.BallModel;
            _inputManager = inputManager;
            _inputManager.InputChange.Subscribe(_ => SubscribeToInput()).AddTo(this);
            _rigidbody = GetComponent<CustomRigidbody>();
            Observable.EveryUpdate().Subscribe(_ => _model.Position.Value = transform.position).AddTo(this);
            Observable.EveryUpdate().Subscribe(_ => _model.Direction.Value = _rigidbody.TestVelocity).AddTo(this);
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
            _inputManager.SetTest();
        }

        private void SetSpeer(float speed)
        {
            _speed = Math.Clamp(speed, 0, 3);
        }

        private void Punch2(bool flag)
        {

            if (flag == false)
            {
                _rigidbody.AddForce(_moveDirection * 50);
            }
        }

    }
}
