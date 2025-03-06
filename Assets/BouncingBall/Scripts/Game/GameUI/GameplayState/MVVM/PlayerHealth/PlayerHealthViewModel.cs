using UniRx;

namespace Assets.BouncingBall.Scripts.Game.GameUI.GameplayState.MVVM.PlayerHealth
{


    public class PlayerHealthViewModel
    {

        private PlayerHealthModel _model;

        public ReactiveProperty<int> CurrentHealth => _model.CurrentHealth;
        public int MaxHealth => _model.MaxHealth;

        public PlayerHealthViewModel(PlayerHealthModel model)
        {
            _model = model;
        }

    }
}
