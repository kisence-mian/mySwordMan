using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteStatus : IApplicationStatus {

    public override void OnEnterStatus()
    {
        OpenUI<FavoriteWindow>();
    }
}
