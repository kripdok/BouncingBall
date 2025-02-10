using System;
using UniRx;

namespace BouncingBall.Utilities
{

    public class LevelLoaderMediator
    {
        public readonly ReadOnlyReactiveProperty<string> LevelName;

        private readonly ReactiveProperty<string> _levelName = new();
        private readonly ReactiveProperty<bool> _onLevelLoaded = new();


        public LevelLoaderMediator()
        {
            LevelName = new(_levelName);
        }
        public IObservable<bool> OnLevelLoaded => _onLevelLoaded;

        public void SetLevelName(string levelName)
        {
            _levelName.Value = levelName;
            _onLevelLoaded.Value = false;
        }

        public void NotifyLevelIsLoaded()
        {
            _onLevelLoaded.Value = true;
        }
    }
}
