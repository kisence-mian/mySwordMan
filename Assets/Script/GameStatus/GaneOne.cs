using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOne : IApplicationStatus 
{
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
        UIManager.OpenUIWindow<GameUIWindow>();
    }
}
