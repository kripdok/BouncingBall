using BouncingBall.Scripts.DataSystem;
using Cysharp.Threading.Tasks;

public interface IDataLoader
{
    public UniTask<T> LoadDataAsync<T>(string path) where T : IDownloadable, new();

}
