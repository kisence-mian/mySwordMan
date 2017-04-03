using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOne : IApplicationStatus 
{
    GameOneMainMenuWindow m_main;
    CountDownWindow m_count;
    GameUIWindow m_gameUI;
    GameFinishWindow m_finish;

    public override void OnEnterStatus()
    {
        m_main = UIManager.OpenUIWindow<GameOneMainMenuWindow>();
        RhythmLibrary.Init();
        PuzzleService.Init();
    }

    public void StartNormalModel()
    {
        CloseMainUI();
        m_count = UIManager.OpenUIWindow<CountDownWindow>();
    }
    
    public void FinishCountDown()
    {
        GameLogic.Init();
        m_gameUI = UIManager.OpenUIWindow<GameUIWindow>();
    }

    public void PlayAgain()
    {
        CloseGameUI();
        CloseFinsihUI();

        m_count = UIManager.OpenUIWindow<CountDownWindow>();
    }

    public void GameBackMenu()
    {
        CloseGameUI();
        CloseFinsihUI();
        

        UIManager.OpenUIWindow<GameOneMainMenuWindow>();
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            UIManager.OpenUIWindow<GameFinishWindow>();
        }
    }

    public void ExaminePoem(poemDataGenerate data)
    {
        CloseGameUI();
        CloseFinsihUI();

        ExaminePoemWindow.s_poemData = data;
        UIManager.OpenUIWindow<ExaminePoemWindow>();
    }

    public void OpenFinishUI()
    {
        if (m_finish == null)
        {
            m_finish = UIManager.OpenUIWindow<GameFinishWindow>();
        }
    }

    void CloseMainUI()
    {
        if (m_main != null)
        {
            UIManager.CloseUIWindow<GameOneMainMenuWindow>();
            m_main = null;
        }
    }

    void CloseGameUI()
    {
        if (m_gameUI != null)
        {
            UIManager.CloseUIWindow<GameUIWindow>();
            m_gameUI = null;
        }
    }

    void CloseFinsihUI()
    {
        if (m_finish != null)
        {
            UIManager.CloseUIWindow<GameFinishWindow>();
            m_finish = null;
        }
    }

    void CloseExaminePoem()
    {

    }
}
