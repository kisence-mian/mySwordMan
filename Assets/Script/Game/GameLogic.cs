using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic 
{
    GameQuestion m_currentQuestion;

    public void Init()
    {

    }

    //public GameQuestion GetGameInfo()
    //{

    //}

    public void SetAnswer(int answerIndex)
    {

    }
}

struct GameQuestion
{
    string m_question;
    string[] m_answers;

    float m_time;
}
