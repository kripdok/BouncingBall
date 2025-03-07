using System;
using UniRx;

namespace BouncingBall.Utilities
{
    public class LevelLoaderMediator
    {
        public readonly ReadOnlyReactiveProperty<string> CurrentLevelName;
        private readonly ReactiveProperty<string> _currentLevelName = new();
        private readonly ReactiveProperty<bool> _isLevelLoaded = new();

        public IObservable<bool> IsLevelLoaded => _isLevelLoaded;

        public LevelLoaderMediator()
        {
            CurrentLevelName = new(_currentLevelName);
        }

        public void SetLevelName(string levelName)
        {
            _isLevelLoaded.Value = false;
            _currentLevelName.Value = levelName;
        }

        public void NotifyLevelIsLoaded()
        {
            _isLevelLoaded.Value = true;
        }
    }
}
