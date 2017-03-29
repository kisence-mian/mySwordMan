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

        LevelJudge(isError); ;

        return isError;
    }

    static void CreateAnswer()
    {
        string rhythm = RhythmLibrary.GetRhythmID(currentPoemData.m_content[s_currentLine]);

        Debug.Log("当前声韵: "+currentPoemData.m_content[s_currentLine]+" " + rhythm);

        s_correctIndex = GetRandomIndex(true);

        //正确答案
        m_questions[s_correctIndex] = currentPoemData.m_content[s_currentLine];

        //错误答案
        m_questions[GetRandomIndex(false)] = GetErrorAnswer(rhythm);
        m_questions[GetRandomIndex(false)] = GetErrorAnswer(rhythm);
        m_questions[GetRandomIndex(false)] = GetErrorAnswer(rhythm);

        ////有一句错误答案取自本诗
        //if(currentPoemData.m_content.Length - s_currentLine > 5)
        //{
        //    m_questions[GetRandomIndex(false)] = GetErrorAnswerSelfPoem();
        //}
        //else
        //{
        //    m_questions[GetRandomIndex(false)] = GetErrorAnswer(rhythm);
        //}
        
        GlobalEvent.DispatchEvent(GameEventEnum.QuestionChange);
    }

    static void LevelJudge(bool isError)
    {
        if(isError)
        {
            ComboCount = 0;
            Score -= 30;

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

    static string GetErrorAnswer(string rhythm)
    {
        //随机取一首诗的一句
        int random = RandomService.GetRand(0, s_poemdata.TableIDs.Count);
        string poemID = s_poemdata.TableIDs[random];

        poemDataGenerate tmp = DataGenerateManager<poemDataGenerate>.GetData(poemID);
        random = RandomService.GetRand(0, tmp.m_content.Length);

        //空格和重复，再重新随机,韵律也需要相同
        while (tmp.m_content[random].Contains("space") 
            || tmp.m_content[random] == GetCurrentContent()

            || tmp.m_content[random] == m_questions[0]
            || tmp.m_content[random] == m_questions[1]
            || tmp.m_content[random] == m_questions[2]
            || tmp.m_content[random] == m_questions[3]
            ||(rhythm !="" &&  RhythmLibrary.GetRhythmID(tmp.m_content[random]) != ( rhythm))
            || tmp.m_content[random].Length != GetCurrentContent().Length
            )
        {
            random = RandomService.GetRand(0, s_poemdata.TableIDs.Count);
            poemID = s_poemdata.TableIDs[random];

            tmp = DataGenerateManager<poemDataGenerate>.GetData(poemID);
            random = RandomService.GetRand(0, tmp.m_content.Length);
        }
        bool boolTmp = (rhythm !="" &&  RhythmLibrary.GetRhythmID(tmp.m_content[random]) != ( rhythm)) ;
        Debug.Log(boolTmp+ "　" +tmp.m_content[random] +" "+ RhythmLibrary.GetRhythmID(tmp.m_content[random]));

        return tmp.m_content[random];
    }

    static string GetErrorAnswerSelfPoem()
    {
        int random = RandomService.GetRand(s_currentLine, currentPoemData.m_content.Length);

        //空格和重复，再重新随机
        while (currentPoemData.m_content[random].Contains("space")
            || currentPoemData.m_content[random] == GetCurrentContent()

            || currentPoemData.m_content[random] == m_questions[0]
            || currentPoemData.m_content[random] == m_questions[1]
            || currentPoemData.m_content[random] == m_questions[2]
            || currentPoemData.m_content[random] == m_questions[3]
            )
        {
            random = RandomService.GetRand(0, currentPoemData.m_content.Length);
        }

        Debug.Log(currentPoemData.m_content[random] + " " + RhythmLibrary.GetRhythmID(currentPoemData.m_content[random]));

        return currentPoemData.m_content[random];
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
