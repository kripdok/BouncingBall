using System;
using UniRx;

namespace BouncingBall.Utilities
{

    public class LevelLoaderMediator
    {
        public readonly ReadOnlyReactiveProperty<string> LevelName;
        private readonly ReactiveProperty<string> _levelName = new ReactiveProperty<string>();
        public IObservable<bool> OnLevelLoaded => _onLevelLoaded;
        private readonly ReactiveProperty<bool> _onLevelLoaded = new ReactiveProperty<bool>();
        public CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public LevelLoaderMediator()
        {
            LevelName = new(_levelName);
        }

        public void SetLevelName(string levelName)
        {
            _levelName.Value = levelName;
            _onLevelLoaded.Value = false;
        }

        public void LoadLevelAsync()
        {
            _onLevelLoaded.Value = true;
        }
    }
}
