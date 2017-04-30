using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelectState : IApplicationStatus
{

    public override void OnEnterStatus()
    {
        OpenUI<ModeSelectWindow>();
    }

    public override void OnUpdate()
    {
        if (Application.platform == RuntimePlatform.Android
            && (Input.GetKeyDown(KeyCode.Escape))
            || Input.GetKeyDown(KeyCode.Q))
        {
            ApplicationStatusManager.EnterStatus<MainMenuState>();
        }
    }

    public void Back()
    {
        ApplicationStatusManager.EnterStatus<MainMenuState>();
    }

    public void NormalModel()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        string difficultys = "";
        for (int i = 0; i < GameOptionService.DifficultyLevels.Count; i++)
        {
            difficultys += "|" + GameOptionService.DifficultyLevels[i];
        }

        string types = "";

        for (int i = 0; i < GameOptionService.PoemTypes.Count; i++)
        {
            types += "|" + GameOptionService.PoemTypes[i];
        }

        data.Add("GameType", "normal");
        data.Add("DifficultyLevels", difficultys);
        data.Add("PoemTypes", types);

        SDKManager.Log("StartGame", data);

        GameLogic.s_GameModel = GameModel.normal;
        PoemLibrary.SetPoemByTag(GameOptionService.DifficultyLevels, GameOptionService.PoemTypes);
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }

    public void ArcadeModel()
    {
                Dictionary<string, object> data = new Dictionary<string, object>();

        string difficultys = "";
        for (int i = 0; i < GameOptionService.DifficultyLevels.Count; i++)
        {
            difficultys += "|" + GameOptionService.DifficultyLevels[i];
        }

        string types = "";

        for (int i = 0; i < GameOptionService.PoemTypes.Count; i++)
        {
            types += "|" + GameOptionService.PoemTypes[i];
        }

        data.Add("GameType", "normal");
        data.Add("DifficultyLevels", difficultys);
        data.Add("PoemTypes", types);

        SDKManager.Log("StartGame", data);

        GameLogic.s_GameModel = GameModel.Arcade;
        PoemLibrary.SetPoemByTag(GameOptionService.DifficultyLevels, GameOptionService.PoemTypes);
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }
}
