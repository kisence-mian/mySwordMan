using UnityEngine;
using System.Collections;

public class MainMenuState : IApplicationStatus
{
    bool isInit = false;

    public override void OnEnterStatus()
    {
        OpenUI<GameOneMainMenuWindow>();
    }
}
