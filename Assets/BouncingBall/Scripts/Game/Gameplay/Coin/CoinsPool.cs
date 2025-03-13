using BouncingBall.Game.Data;
using Zenject;

namespace BouncingBall.Game.Gameplay.Coins
{
    public class CoinsPool : MemoryPool<Coin>
    {
        [Inject] GameDataProvider _gameDataManager;

        protected override void Reinitialize(Coin item)
        {
            base.Reinitialize(item);
            item.SetData(new CoinData(_gameDataManager.GameData.NominalCoin));
            item.Reset();
        }
    }
}
