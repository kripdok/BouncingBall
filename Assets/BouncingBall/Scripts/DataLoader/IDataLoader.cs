using Cysharp.Threading.Tasks;

namespace BouncingBall.DataLoader
{
    public interface IDataLoader
    {
        public UniTask<T> LoadDataAsync<T>(string path) where T : IDownloadable, new();
    }
}

