using UnityEngine;
using System.Collections;

public class MainMenuWindow : UIWindowBase 
{
    GameBaseBoardLogic m_GBBL;

    //UI的初始化请放在这里
    public override void OnInit()
    {
        m_GBBL = GlobalLogicManager.GetLogic<GameBaseBoardLogic>();
        //Debug.Log("Init");
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    void Update()
    {
        GetGameObject("Text_title").GetComponent<RectTransform>().anchoredPosition3D
            = m_GBBL.GetPos(6);

        GetGameObject("Button_login").GetComponent<RectTransform>().anchoredPosition3D
            = m_GBBL.GetPos(2);
    }

    public void OnLogin()
    {
        m_GBBL.NextPage();
    }

    public void OnLogionPress(bool isPress)
    {
        m_GBBL.SetPress(2,isPress);
    }


    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //AnimSystem.uguiAlpha(gameObject, 0, 1, 1, InteType.Linear, true,(object[] obj)=>
        //{
        //    l_animComplete(this, l_callBack, objs);
        //});

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //AnimSystem.uguiAlpha(gameObject, 1, 0, 1, InteType.Linear, true, (object[] obj) =>
        //{
        //    l_animComplete(this, l_callBack, objs);
        //});

        yield return new WaitForEndOfFrame();
    }
}