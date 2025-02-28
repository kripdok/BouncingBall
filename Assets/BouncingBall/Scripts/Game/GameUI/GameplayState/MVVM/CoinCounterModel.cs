using BouncingBall.Game.Data;
using UniRx;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class CoinCounterModel
    {
        public ReactiveProperty<int> CoinCount;

        public CoinCounterModel(PlayerData playerData)
        {
            CoinCount = playerData.CoinsCount;
        }

        public void AddCoin(int amount)
        {
            CoinCount.Value += amount;
        }

        public void RemoveCoin(int amount)
        {
            CoinCount.Value -= amount;
        }
    }
}
