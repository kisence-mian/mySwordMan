using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic 
{
    public static poemDataGenerate currentPoemData;
    static DataTable s_poemdata;

    public static string[] m_questions = new string[4];
    public static float m_time;

    static int s_currentLine = 0;
    static int s_correctIndex = 0;

    static int s_score = 0;
    static int s_comboCount = 0;

    static float s_questionTime = 0;

    public static int ComboCount
    {
        get { return GameLogic.s_comboCount; }
        set { GameLogic.s_comboCount = value; }
    }

    public static int Score
    {
        get { return GameLogic.s_score; }
        set { GameLogic.s_score = value;

        if (s_score <0)
            {
                s_score = 0;
            }
        GlobalEvent.DispatchEvent(GameEventEnum.ScoreChange);
        }
    }
    static int s_hp = 0;

    public static int HP
    {
        get { return GameLogic.s_hp; }
        set { GameLogic.s_hp = value;
        GlobalEvent.DispatchEvent(GameEventEnum.HpChange);
        }
    }

    public static void Init()
    {
        s_score = 0;
        s_comboCount = 0;
        s_questionTime = Time.time;
        s_poemdata = DataManager.GetData("poemData");

        int random = RandomService.GetRand(0, s_poemdata.TableIDs.Count);
        string poemID = s_poemdata.TableIDs[random];

        currentPoemData = DataGenerateManager<poemDataGenerate>.GetData(poemID);

        s_currentLine = 2;

        CreateAnswer();
    }

    public static void NextLine()
    {
        s_currentLine++;
        s_questionTime = Time.time;

        if (s_currentLine < currentPoemData.m_content.Length)
        {
            if (currentPoemData.m_content[s_currentLine].Contains("space"))
            {
                GlobalEvent.DispatchEvent(GameEventEnum.CreateSpace);
                s_currentLine++;
            }

            CreateAnswer();
        }
        else
        {
            UIManager.OpenUIWindow<GameFinishWindow>();
        }
    }

    public static string GetCurrentContent()
    {
        return currentPoemData.m_content[s_currentLine];
    }

    public static bool SetAnswer(int answerIndex)
    {
        bool isError = false;

        if (answerIndex == s_correctIndex)
        {
            isError =  false;
        }
        else
        {
            isError = true;
        }

        LevelJudge(isError);

        return isError;
    }

    static void CreateAnswer()
    {
        PuzzleService.Reset();

        //去掉已出的所有句子
        for (int i = 0; i <= s_currentLine; i++)
        {
            PuzzleService.RemoveSentence(currentPoemData.m_content[i]);
        }

        PuzzleService.RemoveSentence(GetCurrentContent());

        s_correctIndex = GetRandomIndex(true);

        //正确答案
        m_questions[s_correctIndex] = GetCurrentContent();

        //错误答案
        m_questions[GetRandomIndex(false)] = PuzzleService.GetErrorAnswer(GetCurrentContent());
        m_questions[GetRandomIndex(false)] = PuzzleService.GetErrorAnswer(GetCurrentContent());
        m_questions[GetRandomIndex(false)] = PuzzleService.GetErrorAnswer(GetCurrentContent());

        GlobalEvent.DispatchEvent(GameEventEnum.QuestionChange);
    }

    static void LevelJudge(bool isError)
    {
        if(isError)
        {
            ComboCount = 0;
            //Score -= 30;

            GlobalEvent.DispatchEvent(GameEventEnum.ShowScoreLevel, ScoreLevel.bad);
        }
        else
        {
            ComboCount++;

            float useTime = Time.time - s_questionTime;
            int scoreTmp = 0;

            ScoreLevel level = ScoreLevel.normal;

            if (useTime < 2)
            {
                level = ScoreLevel.perfect;
                scoreTmp = 50;
            }
            else if(useTime < 4)
            {
                level = ScoreLevel.nice;
                scoreTmp = 20;
            }
            else if (useTime < 8)
            {
                level = ScoreLevel.good;
                scoreTmp = 10;
            }
            else
            {
                ComboCount = 0;
                level = ScoreLevel.normal;
                scoreTmp = 5;
            }

            Score += ComboCount * scoreTmp;

            GlobalEvent.DispatchEvent(GameEventEnum.ShowScoreLevel, level);
        }
    }

    static List<int> s_randomList = new List<int>();
    static int GetRandomIndex(bool isReset)
    {
        if(isReset)
        {
            s_randomList.Clear();
            for (int i = 0; i < 4; i++)
            {
                s_randomList.Add(i);
            }
        }

        int random = RandomService.GetRand(0, s_randomList.Count);
        int result = s_randomList[random];
        s_randomList.RemoveAt(random);

        return result;
    }

}
enum GameEventEnum
{
    QuestionChange,
    CreateSpace,
    GameOver,
    GameFinsih,
    NextPoem,

    ScoreChange,
    ShowScoreLevel,
    HpChange,
}

enum ScoreLevel
{
    bad,
    normal,
    good,
    nice,
    perfect,
}
