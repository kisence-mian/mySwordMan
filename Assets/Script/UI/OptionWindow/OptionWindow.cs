using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionWindow : UIWindowBase 
{
    List<Dictionary<string, object>> m_content = new List<Dictionary<string, object>>();

    const string c_poemLbDataName = "PoemTypeData";

    public override void OnInit()
    {
        DataTable data = DataManager.GetData(c_poemLbDataName);
        m_content.Clear();

        for (int i = 0; i < data.TableIDs.Count; i++)
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            item.Add("poemLib", data.TableIDs[i]);
            m_content.Add(item);
        }

        GetReusingScrollRect("layout_poemLibrary").Init(UIEventKey,"OptionPoemItem");
        GetReusingScrollRect("layout_poemLibrary").SetData(m_content);
    }


    //UI的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("Button_return", OnClickReturn);
        AddOnClickListener("Button_Simplified", OnClickSimplified);
        AddOnClickListener("Button_Traditional", OnClickTraditional);
        AddOnClickListener("Button_english", OnClickEnglish);
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

    void OnClickSimplified(InputUIOnClickEvent e)
    {
        LanguageManager.SetLanguage(SystemLanguage.ChineseSimplified);
    }

    void OnClickTraditional(InputUIOnClickEvent e)
    {
        LanguageManager.SetLanguage(SystemLanguage.ChineseTraditional);
    }

    void OnClickEnglish(InputUIOnClickEvent e)
    {
        LanguageManager.SetLanguage(SystemLanguage.English);
    }
}