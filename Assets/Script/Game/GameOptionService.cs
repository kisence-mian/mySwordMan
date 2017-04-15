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
    const string c_poemTypesKey = "PoemTypes";

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
    }

    static void SaveOption()
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
    }

    public static List<string> GetTags()
    {
        List<string> tags = new List<string>();

        tags.AddRange(s_difficultyLevels);
        tags.AddRange(s_poemTypes);

        return tags;
    }

}

public enum GameOptionEventEnum
{
    PoemLibChange,
    DifficultyChange,
}