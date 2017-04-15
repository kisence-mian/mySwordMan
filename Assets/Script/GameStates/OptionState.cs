using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionState : IApplicationStatus
{

    public override void OnEnterStatus()
    {
        OpenUI<OptionWindow>();
    }
}
