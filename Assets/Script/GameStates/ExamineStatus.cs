using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        OpenUI<ExaminePoemWindow>();
    }

}
