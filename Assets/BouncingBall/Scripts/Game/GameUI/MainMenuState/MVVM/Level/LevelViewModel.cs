using UniRx;

namespace BouncingBall.Game.UI.MVVM.Level
{
    public class LevelViewModel
    {
        public readonly string LevelName;
        public readonly ReactiveCommand<string> StartLevelCommand;
        public readonly ReactiveCommand Clicked;

        private readonly LevelModel _levelModel;

        public LevelViewModel(LevelModel levelModel)
        {
            _levelModel = levelModel;
            LevelName = _levelModel.LevelName;
            StartLevelCommand = new();
            Clicked = new();
            Clicked.Subscribe(_ => StartLevelCommand.Execute(_levelModel.LevelName));
        }
    }
}
