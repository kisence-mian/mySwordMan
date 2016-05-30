using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class talkWindow : MonoBehaviour 
{
    public Text talkContent;
    public Image headIcon;

    public int lineIndex = 0;      //哪一行
    public int characterIndex = 0; //哪一个字


    public List<TalkData> talkList = new List<TalkData>();

    public float currentTalkTime = 0;
    public float characterTimeSpace = 0.1f;

    public bool isWaitClick = false;
    public bool isComplete = false;

    public talkCompleteCallBack callBack;
    //public

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isComplete)
        {
            talkLogic();
        }
	}

    bool isClick = false;
    void talkLogic()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isClick = true;
        }

        if (isWaitClick == true)
        {
            if(isClick)
            {
                isWaitClick = false;
                isClick = false;
            }
            else
            {
                return;
            }
        }

        currentTalkTime -= Time.deltaTime;
        if (currentTalkTime <= 0)
        {
            currentTalkTime = characterTimeSpace;

            if (isClick)
            {
                characterIndex = talkList[lineIndex].talkContent.Length;
            }


            showText();

            characterIndex++;
            if (characterIndex > talkList[lineIndex].talkContent.Length)
            {
                characterIndex = 0;
                lineIndex++;
                isWaitClick = true;
            }

            if (lineIndex >= talkList.Count)
            {
                isComplete = true;
                if (callBack != null)
                {
                    callBack();
                }
            }
        }

        isClick = false;
    }

    void showText()
    {
        if (lineIndex >= talkList.Count || characterIndex > talkList[lineIndex].talkContent.Length)
        {
            return;
        }

        string showCintent = talkList[lineIndex].talkContent.Substring(0, characterIndex);

        for (int i = characterIndex; i < talkList[lineIndex].talkContent.Length; i++)
        {
            showCintent += "　";
        }

        talkContent.text = showCintent;
    }
}
public delegate void talkCompleteCallBack(params object[] args);
