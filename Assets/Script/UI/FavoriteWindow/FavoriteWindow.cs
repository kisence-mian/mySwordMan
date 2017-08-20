using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FavoriteWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnInit()
    {
        //SDKManager.PlayAD(ADType.Banner);

        GetReusingScrollRect("scrollRect").Init(UIEventKey, "FavoriteItem");
    }

    public override void OnOpen()
    {
        List<string> list = FavoritesService.GetFavoritesList();
        if (list.Count > 0)
        {
            GetGameObject("Button_exercise").SetActive(true);
        }
        else
        {
            GetGameObject("Button_exercise").SetActive(false);
        }

        AddOnClickListener("Button_return", OnClickReturn);
        AddOnClickListener("Button_exercise", OnClickExercise);
        SetData();
    }

    public override void OnClose()
    {
        SDKManager.CloseAD(ADType.Banner);
    }

    void SetData()
    {
        List<string> list = FavoritesService.GetFavoritesList();

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        for (int i = 0; i < list.Count; i++)
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            poemDataGenerate poemData = DataGenerateManager<poemDataGenerate>.GetData(list[i]);

            item.Add("poemData", poemData);

            data.Add(item);
        }

        GetReusingScrollRect("scrollRect").SetData(data);
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        m_uiRoot.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        m_uiRoot.SetActive(true);

        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    void OnClickReturn(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.EnterStatus<MainMenuState>();
    }

    void OnClickExercise(InputUIOnClickEvent e)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("GameType", "FavoritesList");
        data.Add("FavoritesListCount", FavoritesService.GetFavoritesList().Count);
        SDKManager.Log("StartGame", data);

        UIManager.CloseUIWindow(this);

        GameLogic.s_GameModel = GameModel.normal;
        PoemLibrary.SetPoemByFavorite(FavoritesService.GetFavoritesList());
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }
}