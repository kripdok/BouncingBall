using UnityEngine;

namespace BouncingBall.Scripts.Game.Gameplay.MainMenu.UI
{
    public class MainMenuUI : StateUI
    {

        [SerializeField] private Transform _levelViewContainer;

        //private readonly ReactiveCollection<LevelViewModel> _levelViewModels = new();

        //[Inject] public LevelViewFactory LevelViewFactory;
        //[Inject] private LevelLoaderMediator _levelLoaderMediator;

        //private List<LevelModel> _levelModels;

        //private void Awake()
        //{
        //    _levelModels = new() { new LevelModel("1") };

        //    _levelViewModels.ObserveAdd().Subscribe(levelViewModel =>
        //    {
        //        levelViewModel.Value.StartLevelCommand.Subscribe(levelName => StartLevelCommand(levelName)).AddTo(this);
        //    }).AddTo(this);

        //    foreach (var levelModel in _levelModels)
        //    {
        //        var viewModel = new LevelViewModel(levelModel);
        //        var obj = LevelViewFactory.Create(_levelViewContainer,viewModel);
        //        _levelViewModels.Add(viewModel);
        //    }
        //}

        //private void StartLevelCommand(string levelName)
        //{
        //    _levelLoaderMediator.SetLevelName(levelName);
        //    OnExit.Invoke();
        //}
    }
}
