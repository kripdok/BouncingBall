using BouncingBall.Game.Data;
using BouncingBall.Game.UI.MVVM.Level;
using BouncingBall.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState
{
    public class MainMenuUI : StateUI
    {
        [SerializeField] private Transform _levelViewContainer;

        [Inject] private LevelViewFactory LevelViewFactory;
        [Inject] private GameDataManager _gameDataManager;

        private readonly ReactiveCollection<LevelViewModel> _levelViewModels = new();

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
                var model = new LevelModel(levelData);
                var viewModel = new LevelViewModel(model);
                var obj = LevelViewFactory.Create(_levelViewContainer, viewModel);
                _levelViewModels.Add(viewModel);
            }
        }

        private void StartLevelCommand(string levelName)
        {
            OnExit.OnNext(levelName);
        }
    }
}
