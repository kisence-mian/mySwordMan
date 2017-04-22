using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteStatus : IApplicationStatus {

    public override void OnEnterStatus()
    {
        OpenUI<FavoriteWindow>();
    }

    public override void OnUpdate()
    {
        if(Application.platform == RuntimePlatform.Android 
            &&(Input.GetKeyDown(KeyCode.Escape))
            || Input.GetKeyDown(KeyCode.Q))
        {
            ApplicationStatusManager.EnterStatus<MainMenuState>();
        }
    }
}
