using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotUpdateState : IApplicationStatus 
{
    HotUpdateWindow m_window;

    public override void OnEnterStatus()
    {
        SDKManager.Init();
        //SDKManager.LoadAD(ADType.Banner);
        SDKManager.LoadAD(ADType.Interstitial);

        FavoritesService.Init();
        PoemLibrary.Init();

        LanguageManager.Init();
        GameOptionService.Init();

        SDKManager.Log("LaunchGame", null);

        m_window = UIManager.OpenUIWindow<HotUpdateWindow>();

        HotUpdateManager.StartHotUpdate(ReceviceHotUpdateProgress);
    }

    public override void OnExitStatus()
    {
        UIManager.CloseUIWindow(m_window);
    }

    void ReceviceHotUpdateProgress(HotUpdateStatusInfo info)
    {
        if(info.m_loadState.isDone)
        {
            ApplicationStatusManager.EnterStatus<MainMenuState>();
        }
        else
        {
            m_window.HotUpdate(info);
        }
    }
}
