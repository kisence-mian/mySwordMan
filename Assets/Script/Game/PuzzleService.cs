using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleService 
{
    static DataTable s_poemdata;
    static Dictionary<string,Dictionary<int,List<string>>> s_library = new Dictionary<string,Dictionary<int,List<string>>>();
    static List<string> s_outSentenceList = new List<string>();

    public static void Init()
    {
        CreatePuzzleLibrary();
    }

    /// <summary>
    /// 构造谜题库
    /// </summary>
    static void CreatePuzzleLibrary()
    {
        s_poemdata = DataManager.GetData("poemData");

        for (int i = 0; i < s_poemdata.TableIDs.Count; i++)
        {
            PutPoem(DataGenerateManager<poemDataGenerate>.GetData(s_poemdata.TableIDs[i]));
        }
    }

    /// <summary>
    /// 把一首诗放入谜题库
    /// </summary>
    /// <param name="poem"></param>
    static void PutPoem(poemDataGenerate poem)
    {
        for (int i = 0; i < poem.m_content.Length; i++)
		{
			 if(!poem.m_content[i].Contains("space"))
             {
                 PutSentence(poem.m_content[i]);
             }
		}
    }

    static void PutSentence(string sentence)
    {
        string rhythm = RhythmLibrary.GetRhythmID(sentence);
        int length = sentence.Length;

        if (!s_library.ContainsKey(rhythm))
        {
            s_library.Add(rhythm, new Dictionary<int, List<string>>());
        }
        if (!s_library[rhythm].ContainsKey(length))
        {
            s_library[rhythm].Add(length, new List<string>());
        }

        if (s_library[rhythm][length].Contains(sentence))
        {
            //Debug.LogError("有重复诗句！ " + poem.m_content[i]);
        }
        else
        {
            s_library[rhythm][length].Add(sentence);
        }
    }

    public static void RemoveSentence(string sentence)
    {
        if (sentence.Contains("space"))
        {
            return;
        }

        string rhythm = RhythmLibrary.GetRhythmID(sentence);
        int length = sentence.Length;

        if (!s_library.ContainsKey(rhythm))
        {
            return;
        }
        if (!s_library[rhythm].ContainsKey(length))
        {
            return;
        }

        if (s_library[rhythm][length].Contains(sentence))
        {
            s_library[rhythm][length].Remove(sentence);
            s_outSentenceList.Add(sentence);
        }
    }

    public static void Reset()
    {
        for (int i = 0; i < s_outSentenceList.Count; i++)
        {
            PutSentence(s_outSentenceList[i]);
        }

        s_outSentenceList.Clear();
    }


    /// <summary>
    /// 优先给出韵脚相同并且字数相同的答案，
    /// 否则再给字数相同的答案，
    /// 否则再给韵脚相同的答案，
    /// 否则再给一个随机答案
    /// </summary>
    /// <param name="content"></param>
    public static string GetErrorAnswer(string content)
    {
        string rhythm = RhythmLibrary.GetRhythmID(content);
        int length = content.Length;

        if(s_library.ContainsKey(rhythm))
        {
            //韵脚相同，字数相同
            if (s_library[rhythm].ContainsKey(length) && s_library[rhythm][length].Count > 0)
            {
                return GetRandomAnswer(s_library[rhythm][length]);
            }
            //返回一个字数相同的
            else
            {
                string sameLengthID = GetSameContentLengthRhythmID(length);

                if (sameLengthID != null && s_library[sameLengthID][length].Count > 0)
                {
                    return GetRandomAnswer(s_library[sameLengthID][length]);
                }
                //如果没有与它字数相同的,返回一个韵脚相同的
                else
                {
                    return GetRandomContent(s_library[rhythm]);
                }
            }
        }
        //没有与他韵脚相同的诗句
        else
        {
            //先找有没有字数相同的字句
            string sameLengthID = GetSameContentLengthRhythmID(length);

            if (sameLengthID != null)
            {
                return GetRandomAnswer(s_library[sameLengthID][length]);
            }
            //如果没有与它字数相同的，就随机返回一个结果
            else
            {
                return GetRandomContent(s_library[GetRandomRhythm()]);
            }
        }
    }

    /// <summary>
    /// 返回一个随机诗句
    /// </summary>
    static string GetRandomAnswer(List<string> contents)
    {
        int random = RandomService.GetRand(0, contents.Count);
        string result = contents[random];

        s_outSentenceList.Add(result);
        contents.RemoveAt(random);

        return result;
    }

    /// <summary>
    /// 获取一个随机韵脚
    /// </summary>
    /// <returns></returns>
    static string GetRandomRhythm()
    {
        int random = RandomService.GetRand(0, RhythmLibrary.GetRhythmList().Count);

        return RhythmLibrary.GetRhythmList()[random];
    }


    static List<string> s_RandomrhythmList = new List<string>();
    /// <summary>
    /// 返回一个有与目标相同字数的韵脚ID
    /// </summary>
    /// <returns></returns>
    static string GetSameContentLengthRhythmID(int length)
    {
        //先判断有没有返回结果
        s_RandomrhythmList.Clear();
        foreach (var item in s_library)
        {
            if (item.Value.ContainsKey(length))
            {
                s_RandomrhythmList.Add(item.Key);
            }
        }

        if (s_RandomrhythmList.Count == 0)
        {
            return null;
        }

        //如果有可行的值，则直接返回
        int random = RandomService.GetRand(0, s_RandomrhythmList.Count);
        return s_RandomrhythmList[random];
    }

    static string GetRandomContent(Dictionary<int,List<string>> contents)
    {
        List<int> list = new List<int>(contents.Keys);

        int random = RandomService.GetRand(0,list.Count);
        int count = 0;

        while(contents[list[random]].Count ==0)
        {
            random = RandomService.GetRand(0, list.Count);
            count++;

            if(count > 1000)
            {
                throw new Exception("GetRandomContent Time Out !");
            }
        }

        return GetRandomAnswer(contents[list[random]]);
    }
}
