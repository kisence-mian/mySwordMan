using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFinishWindow : UIWindowBase 
{
    static int ADCount = 0;
    //UI的初始化请放在这里
    public override void OnOpen()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Score", GameLogic.GetScore());
        data.Add("Poem", GameLogic.currentPoemData.m_poemName);
        data.Add("PoemID", GameLogic.currentPoemData.m_key);

        SDKManager.Log("FinishGame", data);

        SetText("Text_finish", GetGameLevelString(GameLogic.GetGameLevel()));
        SetText("Text_score", LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "score", GameLogic.GetScore()));

        if(GameLogic.s_GameModel == GameModel.normal)
        {
            if (GameLogic.GetIsFullCombo())
            {
                SetText("Text_combo", LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "fullCombo"));
            }
            else
            {
                SetText("Text_combo", LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "maxCombo", (GameLogic.MaxCombo+"/"+GameLogic.GetTotalLine())));
            }
        }
        else
        {
            SetText("Text_combo", LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "maxCombo", GameLogic.MaxCombo));
        }

        GetRectTransform("Text_score").anchoredPosition3D = new Vector3(500, -100, 0);

        AddOnClickListener("Button_again", OnClickAgain);
        AddOnClickListener("Button_back", OnBackMenu);

        AddOnClickListener("Button_read", OnExaminePoem);
        //AddOnClickListener("Button_favorites", OnAddFavorites);
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
        SetActive("Text_combo", false);
        SetActive("Button_read", false);
        SetActive("Button_favorites", false);

        AnimSystem.UguiSizeDelta(GetGameObject("Image_BG"), new Vector2(800, 0), new Vector2(800, 200), 0.4f, interp: InterpType.OutBack);

        yield return new WaitForSeconds(0.3f);

        SetActive("Text_finish", true);
        AnimSystem.UguiAlpha(GetGameObject("Text_finish"), 0, 1);
        AnimSystem.Scale(GetGameObject("Text_finish"), new Vector3(4, 4, 4), Vector3.one,interp:InterpType.InOutBack);

        yield return new WaitForSeconds(0.5f);
        SetActive("Text_score", true);
        SetActive("Text_combo", true);
        AnimSystem.UguiMove(GetGameObject("Text_score"), new Vector3(500, -180, 0), new Vector3(-20, -180, 0), interp: InterpType.OutCubic);
        AnimSystem.UguiMove(GetGameObject("Text_combo"), new Vector3(500, -140, 0), new Vector3(-20, -140, 0), interp: InterpType.OutCubic,delayTime:0.2f);

        yield return new WaitForSeconds(0.5f);
        SetActive("Button_again", true);
        SetActive("Button_back", true);

        SetActive("Button_read", true);


        AnimSystem.UguiMove(GetGameObject("Button_back"), new Vector3(0, -100, 0), new Vector3(0, 50, 0), 0.5f, 0.3f, interp: InterpType.OutQuart);
        AnimSystem.UguiMove(GetGameObject("Button_again"), new Vector3(0, -200, 0), new Vector3(0, 120, 0), 0.5f, 0.2f, interp: InterpType.OutQuart);
        AnimSystem.UguiMove(GetGameObject("Button_read"), new Vector3(0, -300, 0), new Vector3(0, 190, 0), 0.5f, 0.1f, interp: InterpType.OutQuart);

        AnimSystem.UguiAlpha(GetGameObject("Button_back"), 0, 1, 0.4f, delayTime: 0.3f);
        AnimSystem.UguiAlpha(GetGameObject("Button_again"), 0, 1, 0.4f, delayTime: 0.2f);
        AnimSystem.UguiAlpha(GetGameObject("Button_read"), 0, 1, 0.4f, delayTime: 0.1f);

        //if (!FavoritesService.GetIsFavorites(GameLogic.currentPoemData.m_key))
        //{
        //    SetActive("Button_favorites", true);
        //    AnimSystem.UguiMove(GetGameObject("Button_favorites"), new Vector3(0, -400, 0), new Vector3(0, 260, 0), 0.5f, 0f, interp: InterpType.OutQuart);
        //    AnimSystem.UguiAlpha(GetGameObject("Button_favorites"), 0, 1, 0.4f, delayTime: 0f);
        //}

        yield return base.EnterAnim(l_animComplete, l_callBack, objs);
    }

    public string GetGameLevelString(GameLevel level)
    {
        switch(level)
        {
            case GameLevel.unfinish: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "unfinish");
            case GameLevel.finish: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "finish");
            case GameLevel.Good: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "GameGood");
            case GameLevel.veryGood: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "GameVeryGood");
            case GameLevel.nice: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "GameNice");
            case GameLevel.perfect: return LanguageManager.GetContent(LanguageManager.c_defaultModuleKey, "GamePerfect");
            default :return "";
        }
    }

    public override void OnCompleteEnterAnim()
    {
        ADCount++;
        if(ADCount >= 5)
        {
            ADCount = 0;
            SDKManager.PlayAD(ADType.Interstitial);
        }

    }

    //UI的退出动画
    public override IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        yield return base.ExitAnim(l_animComplete, l_callBack, objs);
    }

    void OnClickAgain(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.GetStatus<GameStatus>().PlayAgain();
    }

    void OnBackMenu(InputUIOnClickEvent e)
    {
        ApplicationStatusManager.EnterStatus<MainMenuState>();
    }

    //void OnAddFavorites(InputUIOnClickEvent e)
    //{
    //    SetActive("Button_favorites", false);
    //    FavoritesService.AddFavorites(GameLogic.currentPoemData.m_key);
    //    //ApplicationStatusManager.GetStatus<GameOne>().GameBackMenu();
    //}

    void OnExaminePoem(InputUIOnClickEvent e)
    {
        ExaminePoemWindow.s_poemData = GameLogic.currentPoemData ;
        UIManager.OpenUIWindow<ExaminePoemWindow>();
    }
}