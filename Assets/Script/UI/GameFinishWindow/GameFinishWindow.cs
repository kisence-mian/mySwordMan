using UnityEngine;
using System.Collections;

public class GameFinishWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnOpen()
    {
        SetText("Text_score","得分：" +GameLogic.Score);
        GetRectTransform("Text_score").anchoredPosition3D = new Vector3(500, -100, 0);

        AddOnClickListener("Button_again", OnClickAgain);
        AddOnClickListener("Button_back", OnBackMenu);
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        SetActive("Button_again",false);
        SetActive("Button_back", false);
        SetActive("Text_finish", false);
        SetActive("Text_score", false);

        AnimSystem.UguiSizeDelta(GetGameObject("Image_BG"), new Vector2(800, 0), new Vector2(800, 200), 0.4f, interp: InterpType.OutBack);

        yield return new WaitForSeconds(0.3f);

        SetActive("Text_finish", true);
        AnimSystem.UguiAlpha(GetGameObject("Text_finish"), 0, 1);
        AnimSystem.Scale(GetGameObject("Text_finish"), new Vector3(4, 4, 4), Vector3.one,interp:InterpType.InOutBack);

        yield return new WaitForSeconds(0.5f);
        SetActive("Text_score", true);
        AnimSystem.UguiMove(GetGameObject("Text_score"), new Vector3(500, -100, 0), new Vector3(218, -100, 0), interp: InterpType.OutCubic);

        yield return new WaitForSeconds(0.5f);
        SetActive("Button_again", true);
        SetActive("Button_back", true);

        yield return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        //AnimSystem.UguiAlpha(gameObject , null, 0,time:0.2f, callBack:(object[] obj) =>
        //{
        //    StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        //});

        //yield return new WaitForEndOfFrame();

        yield return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    void OnClickAgain(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<GameOne>().PlayAgain();
    }

    void OnBackMenu(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<GameOne>().GameBackMenu();
    }
}