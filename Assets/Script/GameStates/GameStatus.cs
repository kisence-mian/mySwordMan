using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : IApplicationStatus
{
    bool isInit = false;
    public override void OnEnterStatus()
    {
        if (!isInit)
        {
            RhythmLibrary.Init();
            PuzzleService.Init();
        }

        OpenUI<CountDownWindow>();
    }

    public void FinishCountDown()
    {
        GameLogic.Init();

        CloseUI<CountDownWindow>();
        OpenUI<GameUIWindow>();
    }

    public void PlayAgain()
    {
        CloseUI<GameUIWindow>();
        CloseUI<GameFinishWindow>();

        OpenUI<CountDownWindow>();
    }

    public void OpenFinishUI()
    {
        OpenUI<GameFinishWindow>();
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
}
