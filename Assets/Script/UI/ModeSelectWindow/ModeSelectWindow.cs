using UnityEngine;
using System.Collections;

public class ModeSelectWindow : UIWindowBase 
{
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_Arcade", ClickArcadeModel);
        AddOnClickListener("Button_back", ClickBack);
        AddOnClickListener("Button_normal", ClickNormalModel);
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    public void ClickNormalModel(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<ModelSelectState>().NormalModel();
    }

    public void ClickArcadeModel(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<ModelSelectState>().ArcadeModel();
    }

    public void ClickBack(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<ModelSelectState>().Back();
    }
}