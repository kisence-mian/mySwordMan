using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionService  
{
    static List<string> s_difficultyLevels = new List<string>();

    public static List<string> DifficultyLevels
    {
        get { return GameOptionService.s_difficultyLevels; }
        set { GameOptionService.s_difficultyLevels = value; }
    }
    static List<string> s_poemTypes = new List<string>();

    public static List<string> PoemTypes
    {
        get { return GameOptionService.s_poemTypes; }
        set { GameOptionService.s_poemTypes = value; }
    }

    const string c_difficultyRecordKey = "DifficultyRecord";
    const string c_poemTypesKey = "PoemTypesRecord";
    const string c_langeuageRecord = "LanguageRecord";

    const string c_langeuageKey = "Language";

    public static void Init()
    {
        LoadOption();
    }

    static void LoadOption()
    {
        RecordTable difficulty = RecordManager.GetData(c_difficultyRecordKey);
        foreach (var item in difficulty)
        {
            s_difficultyLevels.Add(item.Key);
        }

        if (s_difficultyLevels.Count == 0)
        {
            s_difficultyLevels.Add("normal");
        }

        RecordTable poemTypes = RecordManager.GetData(c_poemTypesKey);
        foreach (var item in poemTypes)
        {
            s_poemTypes.Add(item.Key);
        }

        if (s_poemTypes.Count == 0)
        {
            s_poemTypes.Add("songci");
        }

        RecordTable langeConfig = RecordManager.GetData(c_langeuageRecord);

        SystemLanguage langeType = langeConfig.GetEnumRecord<SystemLanguage>(c_langeuageKey, Application.systemLanguage);
        LanguageManager.SetLanguage(langeType);
    }

    public static void SaveOption()
    {
        RecordTable difficulty = new RecordTable();
        for (int i = 0; i < s_difficultyLevels.Count; i++)
        {
            difficulty.SetRecord(s_difficultyLevels[i], "");
        }
        RecordManager.SaveData(c_difficultyRecordKey, difficulty);

        RecordTable poemTypes = new RecordTable();
        for (int i = 0; i < s_poemTypes.Count; i++)
        {
            poemTypes.SetRecord(s_poemTypes[i], "");
        }
        RecordManager.SaveData(c_poemTypesKey, poemTypes);

        RecordTable langeConfig = new RecordTable();
        langeConfig.SetEnumRecord(c_langeuageKey, LanguageManager.s_currentLanguage);
        RecordManager.SaveData(c_langeuageRecord, langeConfig);
    }

    public static void AddPoemType(string poemType)
    {
        if(!s_poemTypes.Contains(poemType))
        {
            s_poemTypes.Add(poemType);
            GlobalEvent.DispatchEvent(GameOptionEventEnum.PoemLibChange);
            SaveOption();
        }
    }

    public static void RemovePoemType(string poemType)
    {
        if (s_poemTypes.Contains(poemType) && s_poemTypes.Count > 1)
        {
            s_poemTypes.Remove(poemType);
            GlobalEvent.DispatchEvent(GameOptionEventEnum.PoemLibChange);
            SaveOption();
        }
    }

    public static void AddDifficulty(string difficulty)
    {
        if (!DifficultyLevels.Contains(difficulty))
        {
            DifficultyLevels.Add(difficulty);
            GlobalEvent.DispatchEvent(GameOptionEventEnum.DifficultyChange);
            SaveOption();
        }
    }

    public static void RemoveDifficulty(string difficulty)
    {
        if (DifficultyLevels.Contains(difficulty) && DifficultyLevels.Count > 1)
        {
            DifficultyLevels.Remove(difficulty);
            GlobalEvent.DispatchEvent(GameOptionEventEnum.DifficultyChange);
            SaveOption();
        }
    }
}

public enum GameOptionEventEnum
{
    PoemLibChange,
    DifficultyChange,
}