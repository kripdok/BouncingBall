using Assets.BouncingBall.Scripts.InputSystem.CostumInput;
using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class Ball : MonoBehaviour, IPointerDownHandler
    {
        private Rigidbody _body;
        private BallData _model;

        private IInputManager _inputManager;

        [Inject]
        public void Constructor(GameDataManager GameDataManager, IInputManager inputManager)
        {
            _model = GameDataManager.GameData.BallModel;
            _inputManager = inputManager;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputManager.SetTest();
        }

    }
}
