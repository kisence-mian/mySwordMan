using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOne : IApplicationStatus 
{
    GameLogic m_gameLogic = new GameLogic();
    public override void OnEnterStatus()
    {
        UIManager.OpenUIWindow<GameOneMainMenuWindow>();
    }

    public void StartNormalModel()
    {
        UIManager.CloseUIWindow<GameOneMainMenuWindow>();
        UIManager.OpenUIWindow<CountDownWindow>();
    }
    
    public void FinishCountDown()
    {
        GameLogic.Init();
        UIManager.OpenUIWindow<GameUIWindow>();
    }

    public void PlayAgain()
    {
       
        UIManager.CloseUIWindow<GameFinishWindow>();
        UIManager.CloseUIWindow<GameUIWindow>();

        UIManager.OpenUIWindow<CountDownWindow>();
    }

    public void GameBackMenu()
    {
        UIManager.CloseUIWindow<GameFinishWindow>();
        UIManager.CloseUIWindow<GameUIWindow>();

        UIManager.OpenUIWindow<GameOneMainMenuWindow>();
    }

    //public override void OnUpdate()
    //{
    //    if(Input.GetKeyUp(KeyCode.Q))
    //    {
    //        UIManager.OpenUIWindow<GameFinishWindow>();
    //    }
    //}
}
