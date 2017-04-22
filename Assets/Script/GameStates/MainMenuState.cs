using UnityEngine;
using System.Collections;

public class MainMenuState : IApplicationStatus
{
    bool isInit = false;

    public override void OnEnterStatus()
    {
        m_exitCount = 0;
        OpenUI<GameOneMainMenuWindow>();
    }

    int m_exitCount = 0;
    public override void OnUpdate()
    {
        if (Application.platform == RuntimePlatform.Android
            && (Input.GetKeyDown(KeyCode.Escape))
            ||Input.GetKeyDown(KeyCode.Q))
        {
            if (m_exitCount == 1)
            {
                Application.Quit();
            }
            else
            {
                TipWindow ui = UIManager.OpenUIWindow<TipWindow>();
                ui.SetContent("UI", "QuitTips");

                m_exitCount++;
            }
        }
    }
}
