using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemLibrary  
{
    static DataTable s_poemdata;
    static List<poemDataGenerate> s_poemList = new List<poemDataGenerate>();

    public static void Init()
    {
        s_poemdata = DataManager.GetData("poemData");
    }

    public static void SetPoemByAllPoem()
    {
        s_poemList.Clear();

        for (int i = 0; i < s_poemdata.TableIDs.Count; i++)
        {
            s_poemList.Add(DataGenerateManager<poemDataGenerate>.GetData(s_poemdata.TableIDs[i]));
        }
    }

    public static void SetPoemByName(string poemID)
    {
        s_poemList.Clear();
        poemDataGenerate poem = DataGenerateManager<poemDataGenerate>.GetData(poemID);

        s_poemList.Add(poem);
    }

    public static void SetPoemByFavorite(List<string> favoriteList)
    {
        s_poemList.Clear();

        for (int i = 0; i < favoriteList.Count; i++)
        {
            s_poemList.Add(DataGenerateManager<poemDataGenerate>.GetData(favoriteList[i]));
        }
    }

    public static void SetPoemByTag(List<string> tags)
    {

    }

    public static poemDataGenerate GetRandomPoem()
    {
        if (s_poemList.Count > 0)
        {
            int random = RandomService.GetRand(0, s_poemList.Count);
            return s_poemList[random];
        }
        else
        {
            return null;
        }
    }
}
