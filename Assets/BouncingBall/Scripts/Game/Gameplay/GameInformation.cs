using BouncingBall.Scripts.Game.Gameplay.BallSystem;
using BouncingBall.Scripts.Game.Gameplay.LevelSystem;
using System.Collections.Generic;

public class GameInformation
{
    public readonly BallModel BallModel;
    public readonly Dictionary<string, LevelModel> LevelModels;

    public GameInformation()
    {
        BallModel = new BallModel();
        LevelModels = new Dictionary<string, LevelModel>()
        {
            ["0"] = new LevelModel("0"),
            ["1"] = new LevelModel("1"),
        };
    }
}
