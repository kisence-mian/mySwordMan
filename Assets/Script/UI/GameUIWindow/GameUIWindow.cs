using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIWindow : UIWindowBase 
{
    public List<PoemItem> m_itemList = new List<PoemItem>();


    //UI的初始化请放在这里W
    public override void OnOpen()
    {
        GlobalEvent.AddEvent(GameEventEnum.QuestionChange, UpdateQuestion);
        GlobalEvent.AddEvent(GameEventEnum.CreateSpace, AddSpace);
        GlobalEvent.AddEvent(GameEventEnum.ScoreChange, ChangeScore);

        GlobalEvent.AddEvent(GameEventEnum.ShowScoreLevel, ShowScoreLevel);

        AddOnClickListener("Button_answer1", OnClickAnswer, "1");
        AddOnClickListener("Button_answer2", OnClickAnswer, "2");
        AddOnClickListener("Button_answer3", OnClickAnswer, "3");
        AddOnClickListener("Button_answer4", OnClickAnswer, "4");

        SetText("Text_score", LanguageManager.GetContent("score", 0));
    }

    public override void OnClose()
{
        GlobalEvent.RemoveEvent(GameEventEnum.QuestionChange, UpdateQuestion);
        GlobalEvent.RemoveEvent(GameEventEnum.CreateSpace, AddSpace);
        GlobalEvent.RemoveEvent(GameEventEnum.ScoreChange, ChangeScore);

        GlobalEvent.RemoveEvent(GameEventEnum.ShowScoreLevel, ShowScoreLevel);

        for (int i = 0; i < m_itemList.Count; i++)
        {
            GameObjectManager.DestroyPoolObject(m_itemList[i]);
        }

        m_itemList.Clear();
    }

    public override void OnCompleteEnterAnim()
    {
        InitQuestion();
        UpdateQuestion();
    }

    public override void OnRefresh()
    {
        base.OnRefresh();
    }

    void InitQuestion()
    {
        CreatePoemItem(GameLogic.currentPoemData.m_content[0], false, new Vector3(0, 200, 0));
        CreatePoemItem(GameLogic.currentPoemData.m_content[1], false, new Vector3(0, 200, 0));
    }

    #region 动画
    void ShowQusetionEnterAnim()
    {
        SetActive("Button_answer1", true);
        SetActive("Button_answer2", true);
        SetActive("Button_answer3", true);
        SetActive("Button_answer4", true);

        GetGameObject("Button_answer1").transform.localScale = Vector3.one;
        GetGameObject("Button_answer2").transform.localScale = Vector3.one;
        GetGameObject("Button_answer3").transform.localScale = Vector3.one;
        GetGameObject("Button_answer4").transform.localScale = Vector3.one;

        AnimSystem.UguiMove(GetGameObject("Button_answer1"), new Vector3(0, -400, 0), new Vector3(0, -30, 0), 0.5f,0,interp: InterpType.OutQuart);
        AnimSystem.UguiMove(GetGameObject("Button_answer2"), new Vector3(0, -500, 0), new Vector3(0, -100, 0), 0.5f, 0.1f, interp: InterpType.OutQuart);
        AnimSystem.UguiMove(GetGameObject("Button_answer3"), new Vector3(0, -600, 0), new Vector3(0, -170, 0), 0.5f, 0.2f, interp: InterpType.OutQuart);
        AnimSystem.UguiMove(GetGameObject("Button_answer4"), new Vector3(0, -700, 0), new Vector3(0, -240, 0), 0.5f, 0.3f, interp: InterpType.OutQuart);

        AnimSystem.UguiAlpha(GetGameObject("Button_answer1"), 0, 1, 0.4f, delayTime: 0);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer2"), 0, 1, 0.4f, delayTime: 0.1f);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer3"), 0, 1, 0.4f, delayTime: 0.2f);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer4"), 0, 1, 0.4f, delayTime: 0.3f);
    }

    void ShowScoreLevel(ScoreLevel level)
    {
        SetText("Text_scoreLevel", ShowlevelContent(level));

        //if(GameLogic.c)
        if (GameLogic.ComboCount > 2)
        {
            SetText("Text_combo", LanguageManager.GetContent( "combo" , GameLogic.ComboCount));
        }
        else
        {
            SetText("Text_combo", "");
        }

        AnimSystem.StopAnim(GetGameObject("Image_scoreLevel"),false);
        AnimSystem.UguiSizeDelta(GetGameObject("Image_scoreLevel"), new Vector2(400, -1), new Vector2(400, 100) ,repeatCount:2,repeatType:RepeatType.PingPang,interp: InterpType.OutBack);
    }

    string ShowlevelContent(ScoreLevel level)
    {
        switch(level)
        {
            case ScoreLevel.bad: return LanguageManager.GetContent("bad");
            case ScoreLevel.normal: return LanguageManager.GetContent("normal");
            case ScoreLevel.good: return LanguageManager.GetContent("good");
            case ScoreLevel.nice: return LanguageManager.GetContent("nice");
            case ScoreLevel.perfect: return LanguageManager.GetContent("perfect");
            default: return "";
        }
    }

    void HideQusetionAnim(int index)
    {
        float timeSpace = 0.075f;

        float alphaTime = 0.05f;
        float scaleTime = 0.05f;

        AnimSystem.UguiAlpha(GetGameObject("Button_answer1"), 1, 0, alphaTime, delayTime: Mathf.Abs(index) * timeSpace);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer2"), 1, 0, alphaTime, delayTime: Mathf.Abs(1 - index) * timeSpace);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer3"), 1, 0, alphaTime, delayTime: Mathf.Abs(2 - index) * timeSpace);
        AnimSystem.UguiAlpha(GetGameObject("Button_answer4"), 1, 0, alphaTime, delayTime: Mathf.Abs(3 - index) * timeSpace);

        AnimSystem.Scale(GetGameObject("Button_answer1"), Vector3.one, Vector3.zero, scaleTime, delayTime: Mathf.Abs(index) * timeSpace);
        AnimSystem.Scale(GetGameObject("Button_answer2"), Vector3.one, Vector3.zero, scaleTime, delayTime: Mathf.Abs(1 - index) * timeSpace);
        AnimSystem.Scale(GetGameObject("Button_answer3"), Vector3.one, Vector3.zero, scaleTime, delayTime: Mathf.Abs(2 - index) * timeSpace);
        AnimSystem.Scale(GetGameObject("Button_answer4"), Vector3.one, Vector3.zero, scaleTime, delayTime: Mathf.Abs(3 - index) * timeSpace);

        Timer.DelayCallBack(0.5f, (obj) =>
        {
            GameLogic.NextLine();

        });
    }

    #endregion

    #region 功能函数

    void CreatePoemItem(string content,bool isError,Vector3 pos)
    {
        PoemItem pi = (PoemItem)GameObjectManager.GetPoolObject("PoemItem", m_uiRoot);

        m_itemList.Add(pi);

        pi.m_rectTransform.anchoredPosition3D = pos;
        pi.m_currentIndex = m_itemList.Count;

        pi.SetContent(content, isError);
        pi.transform.localScale = Vector3.one;
        pi.m_rectTransform.sizeDelta = new Vector2(0,40);

        pi.StartAnim();

        for (int i = 0; i < m_itemList.Count; i++)
        {
            m_itemList[i].m_totalItemCount = m_itemList.Count;
            m_itemList[i].StartAnim();
        }
    }

    #endregion

    #region 事件处理
    void UpdateQuestion(params object[] objs)
    {
        SetText("Text_answer1", GameLogic.m_questions[0]);
        SetText("Text_answer2", GameLogic.m_questions[1]);
        SetText("Text_answer3", GameLogic.m_questions[2]);
        SetText("Text_answer4", GameLogic.m_questions[3]);

        ShowQusetionEnterAnim();
    }

    void ChangeScore(params object[] objs)
    {
        SetText("Text_score", LanguageManager.GetContent("score",GameLogic.Score));
    }

    void ShowScoreLevel(params object[] objs)
    {
        ShowScoreLevel((ScoreLevel)objs[0]);
    }

    //void ChangeHP(params object[] objs)
    //{
    //    SetText("Text_score", "得分：" + GameLogic.Score);
    //}

    void AddSpace(params object[] objs)
    {
        CreatePoemItem("", false, new Vector3(0, Screen.height / 2, 0));
    }

    void OnClickAnswer(InputUIOnClickEvent e)
    {
        int index = int.Parse(e.m_pram) - 1;

        CreatePoemItem(GameLogic.GetCurrentContent(), GameLogic.SetAnswer(index), GetRectTransform(e.m_compName).anchoredPosition3D - new Vector3(0, Screen.height / 2, 0));

        HideQusetionAnim(index);
    }

    #endregion
}