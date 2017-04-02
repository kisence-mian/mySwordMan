using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoritesService 
{
    const string c_recordName = "FavoritesPoem";
    static List<string> s_favoritesList = new List<string>();

    public static void Init()
    {
        s_favoritesList = new List<string>();

        RecordTable data = RecordManager.GetData(c_recordName);
        foreach(var item in data)
        {
            s_favoritesList.Add(item.Key);
        }
    }

    public static void SaveFavorites()
    {
        RecordTable data = new RecordTable();

        for (int i = 0; i < s_favoritesList.Count; i++)
        {
            data.SetRecord(s_favoritesList[i], "");
        }
        RecordManager.SaveData(c_recordName, data);
    }

    public static List<string> GetFavoritesList()
    {
        return null;
    }

    public static bool GetIsFavorites(string poemID)
    {
        return s_favoritesList.Contains(poemID);
    }

    public static void AddFavorites(string poemID)
    {
        if(!s_favoritesList.Contains(poemID))
        {
            s_favoritesList.Add(poemID);
        }

        SaveFavorites();
    }

    public static void RemoveFavoite(string poemID)
    {
        if (s_favoritesList.Contains(poemID))
        {
            s_favoritesList.Remove(poemID);
        }

        SaveFavorites();
    }
}
