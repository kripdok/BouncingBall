using BouncingBall.Game.Data;
using BouncingBall.Game.Data.ObjectData;
using BouncingBall.InputSystem.Controller;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody _body;
        private BallData _model;

        [Inject]
        public void Constructor(GameDataManager GameDataManager)
        {
            _model = GameDataManager.GameData.BallModel;
        }

     


    }
}
