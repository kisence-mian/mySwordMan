using UnityEngine;
using System.Collections;

public class MainMenuState : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        FavoritesService.Init();
        PoemLibrary.Init();

        OpenUI<GameOneMainMenuWindow>();
    }
}
