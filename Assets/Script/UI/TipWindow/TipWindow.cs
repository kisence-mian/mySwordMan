using UnityEngine;
using System.Collections;

public class TipWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnInit()
    {

    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    public void SetContent(string moudleName,string content,params object[] objs)
    {
        SetText("Text", LanguageManager.GetContent(moudleName, content, objs));
    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiSizeDelta(GetGameObject("Image"), new Vector2(1000, 0), new Vector2(1000, 100), 0.4f, interp: InterpType.OutBack);

        yield return new WaitForSeconds(2);

        yield return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    public override void OnCompleteEnterAnim()
    {
        UIManager.CloseUIWindow(this);
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiSizeDelta(GetGameObject("Image"), new Vector2(1000, 100), new Vector2(1000, 000), 0.3f);

        yield return new WaitForSeconds(0.3f);

        yield return base.ExitAnim(l_animComplete, l_callBack, objs);
    }
}