using BouncingBall.Game.UI.GameplayState.HUD;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class PlayerHealthView : MonoBehaviour
    {

        [SerializeField] private Transform _container;

        [Inject] private PlayerHealthCellFactory _cellFactory;

        private List<PlayerHealthCell> _cells = new List<PlayerHealthCell>();
        private PlayerHealthViewModel _view;


        public void Init(PlayerHealthViewModel view)
        {
            _view = view;

            CreateHealthCells(_view.MaxHealth);

            _view.CurrentHealth.Skip(1).Subscribe(UpdateHealthCells).AddTo(this);
        }

        private void CreateHealthCells(int maxHealth)
        {

            for (int i = 0; i < maxHealth; i++)
            {
                var cell = _cellFactory.Create(_container);
                _cells.Add(cell);
            }
        }

        public void UpdateHealthCells(int currentHealth)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (i < currentHealth)
                {
                    _cells[i].EnableCell();
                }
                else
                {
                    _cells[i].DisableCell();
                }
            }
        }
    }
}
