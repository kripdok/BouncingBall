using Cysharp.Threading.Tasks;
using UniRx;

namespace BouncingBall.Game.UI.GameplayState.MVVM
{
    public class CoinCounterView
    {
        private CoinCounterModel _model;

        public ReactiveProperty<int> CoinCount;

        public CoinCounterView(CoinCounterModel model)
        {
            _model = model;
            CoinCount = new(_model.CoinCount.Value);
        }

        public async void AddCoin(int coin)
        {
            var amount = _model.CoinCount.Value;

            _model.AddCoin(coin);

            for (int i = 0; i < coin; i++)
            {
                CoinCount.Value++;
                await UniTask.WaitForSeconds(0.1f);
            }
        }

        public async void RemoveCoin(int coin)
        {
            var amount = _model.CoinCount.Value;
            _model.RemoveCoin(coin);

            for (int i = 0; i == coin; i++)
            {
                CoinCount.Value--;
                await UniTask.WaitForSeconds(0.5f);
            }
        }
    }
}
