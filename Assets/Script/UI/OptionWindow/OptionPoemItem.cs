using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPoemItem : ReusingScrollItemBase
{
    string m_poemLib = "";
    public override void OnInit()
    {
        base.OnInit();

        GlobalEvent.AddEvent(LanguageEventEnum.LanguageChange, ReceviceLanguageChange);
        GlobalEvent.AddEvent(GameOptionEventEnum.PoemLibChange, RecevicePoemOptionChange);
    }

    public override void SetContent(int index, Dictionary<string, object> data)
    {
        m_poemLib = data["poemLib"].ToString();

        SetText("Text_content", LanguageManager.GetContent(m_poemLib));

        bool isToggle = GameOptionService.PoemTypes.Contains(m_poemLib);

        SetActive("Text_toggle", isToggle);
    }

    void RecevicePoemOptionChange(params object[] objs)
    {
        bool isToggle = GameOptionService.PoemTypes.Contains(m_poemLib);
        SetActive("Text_toggle", isToggle);
    }

    void ReceviceLanguageChange(params object[] objs)
    {
        SetText("Text_content", LanguageManager.GetContent(m_poemLib));
    }
}
