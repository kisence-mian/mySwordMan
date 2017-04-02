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
        SetData(s_poemData);

        SetActive("Button_removeFavorite", FavoritesService.GetIsFavorites(s_poemData.m_key));

        AddOnClickListener("Button_removeFavorite", OnClickRemoveFavorite);
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
        AnimSystem.UguiMove(GetGameObject("Text_author"), new Vector3(172,150,0), new Vector3(172,-80,0), 0.5f, 0, InterpType.OutBack);
        AnimSystem.UguiMove(GetGameObject("Text_poemName"), new Vector3(0, 100, 0), new Vector3(0, -40, 0), 0.5f, 0, InterpType.OutBack);

        if (FavoritesService.GetIsFavorites(s_poemData.m_key))
        {
            AnimSystem.UguiMove(GetGameObject("Button_removeFavorite"), new Vector3(0, -100, 0), new Vector3(0, 180, 0), 0.5f, 0, InterpType.OutBack);
        }

        AnimSystem.UguiMove(GetGameObject("Button_exercise"), new Vector3(0, -100, 0), new Vector3(0, 110, 0), 0.5f, 0, InterpType.OutBack);
        AnimSystem.UguiMove(GetGameObject("Button_return"), new Vector3(0, -100, 0), new Vector3(0, 45, 0), 0.5f, 0, InterpType.OutBack);

        AnimSystem.UguiAlpha(gameObject, 0, 1, callBack:(object[] obj)=>
        {
            StartCoroutine(base.EnterAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        AnimSystem.UguiAlpha(gameObject , null, 0, callBack:(object[] obj) =>
        {
            StartCoroutine(base.ExitAnim(l_animComplete, l_callBack, objs));
        });

        yield return new WaitForEndOfFrame();
    }

    void OnClickRemoveFavorite(InputUIOnClickEvent e)
    {
        FavoritesService.RemoveFavoite(s_poemData.m_key);
    }

    void OnClickExercise(InputUIOnClickEvent e)
    {

    }

    void OnClickReturn(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.EnterStatus<MainMenuState>();
    }
}