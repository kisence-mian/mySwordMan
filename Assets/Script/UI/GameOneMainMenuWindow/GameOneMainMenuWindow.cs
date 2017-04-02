using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOneMainMenuWindow : UIWindowBase 
{
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_GameStart", OnClickNormalModel);
        AddOnClickListener("Button_favorite", OnClickFavorite);

        SetActive("Button_favorite",FavoritesService.GetFavoritesList().Count > 0);

    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(GetGameObject("Text_title"),  new Vector3(0, 200, 0),new Vector3(0,-200,0), 1f,interp:InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Button_GameStart"), new Vector3(0, -1000, 0), new Vector3(0, -256, 0), 1f, interp: InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Button_favorite"), new Vector3(0, -1000, 0), new Vector3(0, -320, 0), 1f, interp: InterpType.OutExpo);

        AnimSystem.UguiAlpha(GetGameObject("Button_GameStart"), 0, 1);
        AnimSystem.UguiAlpha(GetGameObject("Button_favorite"), 0, 1);

        AnimSystem.UguiAlpha(GetGameObject("Text_title"), 0, 1, callBack: (object[] obj) =>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(GetGameObject("Text_title"), null, new Vector3(0, 100, 0),0.3f);
        AnimSystem.UguiMove(GetGameObject("Button_GameStart"), null, new Vector3(0, -600, 0), 0.3f);
        AnimSystem.UguiMove(GetGameObject("Button_favorite"), null, new Vector3(0, -600, 0), 0.3f);

        AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    void OnClickNormalModel(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }

    void OnClickFavorite(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.EnterStatus<FavoriteStatus>();
    }
}