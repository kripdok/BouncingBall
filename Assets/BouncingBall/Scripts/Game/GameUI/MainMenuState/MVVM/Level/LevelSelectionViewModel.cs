using UniRx;

namespace BouncingBall.Game.UI.MainMenuState.MVVM
{
    public class LevelSelectionViewModel
    {
        public readonly string LevelName;
        public readonly ReactiveCommand<string> StartLevelCommand;
        public readonly ReactiveCommand Clicked;

        private readonly LevelSelectionModel _levelModel;

        public LevelSelectionViewModel(LevelSelectionModel levelModel)
        {
            _levelModel = levelModel;
            LevelName = _levelModel.LevelName;
            StartLevelCommand = new();
            Clicked = new();
            Clicked.Subscribe(_ => StartLevelCommand.Execute(_levelModel.LevelName));
        }
    }
}
