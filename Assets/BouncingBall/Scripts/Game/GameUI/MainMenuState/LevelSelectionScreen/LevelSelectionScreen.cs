using BouncingBall.Game.Data;
using BouncingBall.Game.UI.MainMenuState.MVVM;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState.Screen
{
    public class LevelSelectionScreen : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        [Inject] private LevelSelectionViewFactory LevelViewFactory;
        [Inject] private GameDataManager _gameDataManager;

        private readonly ReactiveCollection<LevelSelectionViewModel> _levelViewModels = new();
        private Subject<string> _setLevelRequested = new();

        public IObservable<string> SetLevelRequested => _setLevelRequested;

        private void Awake()
        {
            _levelViewModels.ObserveAdd().Subscribe(levelViewModel =>
            {
                levelViewModel.Value.StartLevelCommand.Subscribe(levelName => StartLevelCommand(levelName)).AddTo(this);
            }).AddTo(this);

            CreateLevelViewModel();
        }

        private void CreateLevelViewModel()
        {
            foreach (var levelData in _gameDataManager.GameData.LevelName)
            {
                var model = new LevelSelectionModel(levelData);
                var viewModel = new LevelSelectionViewModel(model);
                var obj = LevelViewFactory.Create(_container, viewModel);
                _levelViewModels.Add(viewModel);
            }
        }

        private void StartLevelCommand(string levelName)
        {
            _setLevelRequested.OnNext(levelName);
        }
    }
}
