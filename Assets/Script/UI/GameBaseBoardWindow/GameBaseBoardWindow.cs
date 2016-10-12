using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBaseBoardWindow : UIWindowBase 
{
    public ReusingScrollRect m_rsr;

    //UI的初始化请放在这里
    public override void OnInit()
    {
        string itemName = "BaseBoardItem";
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        for (int i = 0; i < 10000; i++)
        {
            data.Add(new Dictionary<string, object>());
        }

        m_rsr.Init(itemName);
        m_rsr.SetData(data);

    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.uguiAlpha(gameObject, 0, 1, 1, InteType.Linear, true, (object[] obj) =>
        {
            l_animComplete(this, l_callBack, objs);
        });

        yield return new WaitForSeconds(5);

    ;
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.uguiAlpha(gameObject, 1, 0, 1, InteType.Linear, true, (object[] obj) =>
        {
            base.ExitAnim(l_animComplete, l_callBack, objs);
        });

        return null;
    }
}