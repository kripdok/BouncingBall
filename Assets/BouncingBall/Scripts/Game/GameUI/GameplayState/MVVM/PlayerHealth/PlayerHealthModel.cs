using BouncingBall.Game.Data;
using UniRx;

namespace Assets.BouncingBall.Scripts.Game.GameUI.GameplayState.MVVM.PlayerHealth
{
    public class PlayerHealthModel
    {
        public ReactiveProperty<int> CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public PlayerHealthModel(GameDataManager gameDataManager)
        {
            MaxHealth = gameDataManager.GameData.BallData.HealthSystem.MaxHealth;
            CurrentHealth = gameDataManager.GameData.BallData.HealthSystem.CurrentHealth;
        }
    }
}
