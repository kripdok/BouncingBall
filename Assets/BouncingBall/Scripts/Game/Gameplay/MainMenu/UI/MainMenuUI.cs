using Assets.BouncingBall.Scripts.Game.Gameplay;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using BouncingBall.Scripts.Game.Gameplay.Root;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Scripts.Game.Gameplay.MainMenu.UI
{
    public class MainMenuUI : StateUI
    {

        [SerializeField] private Transform _levelViewContainer;

        private readonly ReactiveCollection<LevelViewModel> _levelViewModels = new();

        [Inject] public LevelViewFactory LevelViewFactory;
        
        private void Awake()
        {
            _levelViewModels.ObserveAdd().Subscribe(levelViewModel =>
            {
                levelViewModel.Value.StartLevelCommand.Subscribe(levelName => StartLevelCommand(levelName)).AddTo(this);
            }).AddTo(this);

            foreach (var levelModel in GameInformation.LevelModels)
            {
                var viewModel = new LevelViewModel(levelModel.Value);
                var obj = LevelViewFactory.Create(_levelViewContainer,viewModel);
                _levelViewModels.Add(viewModel);
            }
        }

        private void StartLevelCommand(string levelName)
        {
            GameInformation.EnableLevelId = levelName;
            OnExit.Invoke();
        }
    }
}
