using Cysharp.Threading.Tasks;

namespace BouncingBall.DataLoader
{
    public interface IDataLoader
    {
        public UniTask<T> LoadDataFromPathAsync<T>(string path) where T : IDownloadable, new();
    }
}