using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExaminePoemWindow : UIWindowBase 
{
    public static poemDataGenerate s_poemData;

    List<Dictionary<string, object>> m_contentData = new List<Dictionary<string,object>>();

    //UI的初始化请放在这里
    public override void OnInit()
    {
        GetReusingScrollRect("ScrollRect").Init(UIEventKey, "ExainePoemItem");
    }

    public override void OnClose()
    {
        GetReusingScrollRect("ScrollRect").Dispose();
    }

    public override void OnOpen()
    {
        UpdateFavoriteSwitch();
        UIManager.HideOtherUI("ExaminePoemWindow");

        SetData(s_poemData);

        AddOnClickListener("Button_FavoriteSwitch", OnClickFavoriteSwitch);
        AddOnClickListener("Button_exercise", OnClickExercise);
        AddOnClickListener("Button_return", OnClickReturn);
    }

    public void SetData(poemDataGenerate data)
    {
        m_contentData.Clear();

        SetText("Text_poemName", data.m_poemName);
        SetText("Text_author", data.m_author);

        for (int i = 0; i < data.m_content.Length; i++)
        {
            Dictionary<string, object> itemData = new Dictionary<string, object>();
            itemData.Add("content", data.m_content[i]);

            m_contentData.Add(itemData);
        }

        GetReusingScrollRect("ScrollRect").SetData(m_contentData);
    }

    //请在这里写UI的更新逻辑，当该UI监听的事件触发时，该函数会被调用
    public override void OnRefresh()
    {

    }

    //UI的进入动画
    public override IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiMove(GetGameObject("Text_author"), new Vector3(172, 150, 0), new Vector3(172, -80, 0), 0.5f, 0, InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Text_poemName"), new Vector3(0, 100, 0), new Vector3(0, -40, 0), 0.5f, 0, InterpType.OutExpo);

        AnimSystem.UguiMove(GetGameObject("Button_FavoriteSwitch"), new Vector3(0, -100, 0), new Vector3(0, 175, 0), 0.5f, 0, InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Button_exercise"), new Vector3(0, -100, 0), new Vector3(0, 110, 0), 0.5f, 0, InterpType.OutExpo);
        AnimSystem.UguiMove(GetGameObject("Button_return"), new Vector3(0, -100, 0), new Vector3(0, 45, 0), 0.5f, 0, InterpType.OutExpo);

        AnimSystem.UguiMove(GetGameObject("ScrollRect"), new Vector3(0, 800, 0), new Vector3(0, 32.5f, 0), 0.5f, 0, InterpType.OutExpo);

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

    void OnClickFavoriteSwitch(InputUIOnClickEvent e)
    {
        if (FavoritesService.GetIsFavorites(s_poemData.m_key))
        {
            FavoritesService.RemoveFavoite(s_poemData.m_key);
        }
        else
        {
            FavoritesService.AddFavorites(s_poemData.m_key);
        }

        UpdateFavoriteSwitch();
    }

    void UpdateFavoriteSwitch()
    {
        if(FavoritesService.GetIsFavorites(s_poemData.m_key))
        {
            SetText("Text_FavoriteSwitch", "移除收藏");
        }
        else
        {
            SetText("Text_FavoriteSwitch", "加入收藏");
        }
    }

    void OnClickExercise(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow(this);

        PoemLibrary.SetPoemByName(s_poemData.m_key);
        ApplicationStatusManager.EnterStatus<GameStatus>();
    }

    void OnClickReturn(InputUIOnClickEvent e)
    {
        UIManager.ShowOtherUI("ExaminePoemWindow");
        UIManager.CloseUIWindow(this);
    }
}