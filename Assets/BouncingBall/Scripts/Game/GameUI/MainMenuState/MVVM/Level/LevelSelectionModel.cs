using Newtonsoft.Json;

namespace BouncingBall.Game.UI.MainMenuState.MVVM
{
    public class LevelSelectionModel
    {
        [JsonProperty] public readonly string LevelName;

        public LevelSelectionModel(string levelName)
        {
            LevelName = levelName;
        }
    }
}
