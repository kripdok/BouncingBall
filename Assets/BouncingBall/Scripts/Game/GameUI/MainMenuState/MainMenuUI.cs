using BouncingBall.Game.Data;
using BouncingBall.Game.UI.MVVM.Level;
using BouncingBall.UI;
using BouncingBall.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState
{
    public class MainMenuUI : StateUI
    {

        [SerializeField] private Transform _levelViewContainer;

        private readonly ReactiveCollection<LevelViewModel> _levelViewModels = new();

        [Inject] private LevelViewFactory LevelViewFactory;
        [Inject] private LevelLoaderMediator _levelLoaderMediator;
        [Inject] private GameDataManager _gameDataManager;

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
            foreach (var levelData in _gameDataManager.GameData.LevelData)
            {
                var model = new LevelModel(levelData);
                var viewModel = new LevelViewModel(model);
                var obj = LevelViewFactory.Create(_levelViewContainer, viewModel);
                _levelViewModels.Add(viewModel);
            }
        }

        private void StartLevelCommand(string levelName)
        {
            _levelLoaderMediator.SetLevelName(levelName);
            OnExit.OnNext(Unit.Default);
        }
    }
}
