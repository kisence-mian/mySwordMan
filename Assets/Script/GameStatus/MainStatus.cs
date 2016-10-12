using UnityEngine;
using System.Collections;

public class MainStatus : IApplicationStatus 
{

    public override void OnEnterStatus()
    {
        UIManager.OpenUIWindow<MainMenuWindow>();
        GlobalLogicManager.GetLogic<GameBaseBoardLogic>().OpenBoard();
    }

}
