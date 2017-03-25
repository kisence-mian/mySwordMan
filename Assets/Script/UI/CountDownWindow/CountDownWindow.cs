using UnityEngine;
using System.Collections;

public class CountDownWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnInit()
    {

    }

    public override void OnOpen()
    {
        StartCoroutine(StartCountDown());
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    IEnumerator StartCountDown()
    {
        GetGameObject("Text_3").SetActive(true);

        AnimSystem.UguiAlpha(GetGameObject("Text_3"), 0,1);
        AnimSystem.Scale(GetGameObject("Text_3"), new Vector3(3, 3, 3), Vector3.one,interp:InterpType.OutBack, callBack: (obj) => {
            AnimSystem.UguiAlpha(GetGameObject("Text_3"), 1, 0, time: 0.4f, callBack: (obj2) =>
            {
                GetGameObject("Text_3").SetActive(false);
            });
        });

        yield return new WaitForSeconds(1f);

        GetGameObject("Text_2").SetActive(true);

        AnimSystem.UguiAlpha(GetGameObject("Text_2"), 0, 1);
        AnimSystem.Scale(GetGameObject("Text_2"), new Vector3(3, 3, 3), Vector3.one, interp: InterpType.OutBack, callBack: (obj) =>
        {
            AnimSystem.UguiAlpha(GetGameObject("Text_2"),1, 0,time:0.4f, callBack: (obj2) =>
            {
                GetGameObject("Text_2").SetActive(false);
            });
        });

        yield return new WaitForSeconds(1f);

        GetGameObject("Text_1").SetActive(true);

        AnimSystem.UguiAlpha(GetGameObject("Text_1"), 0, 1);
        AnimSystem.Scale(GetGameObject("Text_1"), new Vector3(3, 3, 3), Vector3.one, interp: InterpType.OutBack, callBack: (obj) =>
        {
            AnimSystem.UguiAlpha(GetGameObject("Text_1"), 1, 0, time: 0.4f, callBack: (obj2) =>
            {
                GetGameObject("Text_1").SetActive(false);
            });
        });

        yield return new WaitForSeconds(1f);

        FinsihCountDown();
    }

    void FinsihCountDown()
    {
        ApplicationStatusManager.GetStatus<GameOne>().FinishCountDown();
    }
}