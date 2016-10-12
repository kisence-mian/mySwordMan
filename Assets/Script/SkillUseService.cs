using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillUseService : MonoBehaviour 
{
    string skillCorpus = "今古河山无定局,画角声中,牧马频来去,漫步荒凉谁可语,西风吹老丹枫树,幽怨从前何处诉,"
                       + "铁马金戈,青冢黄昏路,一往情深深几许,深山夕照深秋雨";

    public List<Text> buttonContents = new List<Text>();

    public Text content;

    public bool isShowProblem = false;

    const string questionLine = "___________";

	// Use this for initialization
	void Start ()
    {
        InitSkillSyetem();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isShowProblem)
        {
            showLogic();
        }
	}

    int answer = 0;
    List<string> problemConetnt = new List<string>();



    //初始化技能系统
    void InitSkillSyetem()
    {
        string[] poems = skillCorpus.Split(',');
        List<string> poemList = new List<string>(poems);

        int randomLine = Random.Range(0,poems.Length);

        //构造题目
        int curentLine = randomLine - 2;

        if (randomLine == poems.Length - 1)
        {
            curentLine = randomLine - 3;
        }

        if(curentLine < 0)
        {
            curentLine = 0;
        }

        problemConetnt = new List<string>();

        for (int i = 0; i < 4;i++ )
        {
            Debug.Log(curentLine);
            if (curentLine == randomLine)
            {
                problemConetnt.Add(questionLine);
            }
            else
            {
                problemConetnt.Add( poems[curentLine]);
            }
            curentLine++;
        }
        content.text = "";

        isShowProblem = true;

        //构造错误答案
        answer = Random.Range(0, 4);
        poemList.RemoveAt(randomLine);

        Debug.Log("answer " + answer);

        for (int i = 0; i < 4; i++)
        {
            if (i == answer)
            {

                Debug.Log("answer");
                buttonContents[i].text = poems[randomLine];
            }
            else
            {
                int random = Random.Range(0,poemList.Count);
                buttonContents[i].text = poemList[random];

                poemList.RemoveAt(random);
            }
        }
    }


    public void showLogic()
    {
        if(Input.GetMouseButtonDown(0))
        {
            characterIndex = problemConetnt[lineIndex].Length;
        }

        currentShowTime -= Time.deltaTime;

        if (currentShowTime <= 0)
        {
            currentShowTime = ShowTimeSpace;

            showProblem();

            characterIndex++;

            if (characterIndex > problemConetnt[lineIndex].Length)
            {
                lineIndex++;
                characterIndex = 0;

                if (lineIndex>= problemConetnt.Count)
                {
                    isShowProblem = false;
                }
            }
        }
    }

    float currentShowTime = 0;
    float ShowTimeSpace = 0.05f;

    int characterIndex = 0;
    int lineIndex = 0;
    void showProblem()
    {
        if (lineIndex >= problemConetnt.Count || characterIndex > problemConetnt[lineIndex].Length)
        {
            return;
        }



        string showCintent = "";


        for (int i = 0; i <= lineIndex;i++ )
        {

            if (i != lineIndex)
            {
                showCintent += convertString(problemConetnt[i]);
            }
            else
            {
                string stringTmp = "";

                Debug.Log(i);
                Debug.Log(problemConetnt.Count);

                stringTmp += problemConetnt[i].Substring(0, characterIndex);

                Debug.Log("stringTmp: " + stringTmp);

                for (int j = characterIndex; j < problemConetnt[i].Length; j++)
                {


                    if (problemConetnt[i].Equals(questionLine))
                    {
                        stringTmp += " ";
                    }
                    else
                    {
                        stringTmp += "　";
                    }

                    if (j == problemConetnt[i].Length - 1)
                    {
                        stringTmp += " ";
                    }
                }


                if (characterIndex == problemConetnt[i].Length)
                {
                    stringTmp += ",";
                }

                if (problemConetnt[i].Equals(questionLine))
                {
                    stringTmp = "<color=yellow>" + stringTmp + "</color>";
                }

                showCintent += stringTmp;
            }

        }
 

        content.text = showCintent;
    }

    string convertString(string str)
    {
        if (str.Equals(questionLine))
        {
            return "<color=yellow>" + str + ",\n</color>";
        }
        else
        {
            return str + ",\n";
        }
    }


    public void buttonCallBack(int buttonIndex)
    {
        if (buttonIndex == answer)
        {
            Debug.Log("right !");
        }
        else
        {
            Debug.Log("wrong !");
        }
    }
}
