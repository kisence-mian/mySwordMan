using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FavoriteWindow : UIWindowBase 
{

    //UI的初始化请放在这里
    public override void OnInit()
    {
        GetReusingScrollRect("scrollRect").Init(UIEventKey, "FavoriteItem");
    }

    public override void OnOpen()
    {
        AddOnClickListener("Button_return", OnClickReturn);
        AddOnClickListener("Button_exercise", OnClickExercise);
        SetData();
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
        PoemLibrary.SetPoemByFavorite(FavoritesService.GetFavoritesList());
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }
}