using Cysharp.Threading.Tasks;

public class GameDataManager
{
    public GameData GameData { get; private set; }

    private IDataLoader _dataLoader;

    public GameDataManager(IDataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    public async UniTask LoadGameData()
    {
        GameData = await _dataLoader.LoadDataAsync<GameData>("Assets/Resources/Data.json");
    }
}
