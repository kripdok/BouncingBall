using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    public class CoinsPool : MemoryPool<CoinData, Coin>
    {
        protected override void Reinitialize(CoinData p1, Coin item)
        {
            base.Reinitialize(p1, item);
            item.Reset();
        }
    }
}
