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

            FavoritesService.Init();
            PoemLibrary.Init();
        }

        OpenUI<GameOneMainMenuWindow>();
    }
}
