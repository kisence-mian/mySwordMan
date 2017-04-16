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
        AddOnClickListener("layout", OnClickItem);
    }

    public override void SetContent(int index, Dictionary<string, object> data)
    {
        m_poemLib = data["poemLib"].ToString();

        ResetText();
        ResetToggle();
    }

    void RecevicePoemOptionChange(params object[] objs)
    {
        ResetToggle();
    }

    void ReceviceLanguageChange(params object[] objs)
    {
        ResetText();
    }

    void ResetText()
    {
        SetText("Text_content", LanguageManager.GetContent(m_poemLib));
    }

    void ResetToggle()
    {
        bool isToggle = GameOptionService.PoemTypes.Contains(m_poemLib);

        if (GetGameObject("Text_toggle").activeSelf != isToggle)
        {
            SetActive("Text_toggle", isToggle);

            if (isToggle)
            {
                AnimSystem.StopAnim(GetGameObject("Text_toggle"));
                AnimSystem.UguiAlpha(GetGameObject("Text_toggle"), 0.1f, 1, 0.4f);
                AnimSystem.Scale(GetGameObject("Text_toggle"), Vector3.one * 4, Vector3.one, 0.4f, InterpType.OutBack);
            }
        }
    }

    void OnClickItem(InputUIOnClickEvent e)
    {
        if (GameOptionService.PoemTypes.Contains(m_poemLib))
        {
            GameOptionService.RemovePoemType(m_poemLib);
        }
        else
        {
            GameOptionService.AddPoemType(m_poemLib);
        }
    }
}
