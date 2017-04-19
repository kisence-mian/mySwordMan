using UnityEngine;
using System.Collections;

public class MainMenuState : IApplicationStatus
{
    bool isInit = false;

    public override void OnEnterStatus()
    {
        if (!isInit)
        {
            isInit = true;

            SDKManager.Init();

            SDKManager.LoadAD(ADType.Banner);
            SDKManager.LoadAD(ADType.Interstitial);

            GameOptionService.Init();

            FavoritesService.Init();
            PoemLibrary.Init();

            //LanguageManager.SetLanguage(SystemLanguage.ChineseTraditional);
        }

        OpenUI<GameOneMainMenuWindow>();
    }
}
