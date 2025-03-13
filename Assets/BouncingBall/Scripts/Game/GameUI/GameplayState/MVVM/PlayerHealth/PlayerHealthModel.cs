using BouncingBall.Game.Data;
using UniRx;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class PlayerHealthModel
    {
        public ReactiveProperty<int> CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public PlayerHealthModel(GameDataProvider gameDataManager)
        {
            MaxHealth = gameDataManager.GameData.BallData.HealthSystem.MaxHealth;
            CurrentHealth = gameDataManager.GameData.BallData.HealthSystem.CurrentHealth;
        }
    }
}
