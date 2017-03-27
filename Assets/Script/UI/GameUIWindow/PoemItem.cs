using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoemItem :PoolObject 
{
    public RectTransform m_rectTransform;
    public Text m_text;

    public int m_totalItemCount = 1;
    public int m_currentIndex = 0;

    int m_showCount = 10;

    bool m_isAnim = false;
    float m_timer = 0;
    float m_totalAnimTime = 0.75f;

    float m_minScale = 0.5f;

    float m_minAlpha = 0f;

    public override void OnCreate()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_text = GetComponent<Text>();
    }

    public void SetContent(string content,bool isError = false)
    {
        if(isError)
        {
            m_text.text = "<color=#ff0000>"+content+"</color>";
        }
        else
        {
            m_text.text = content;
        }
    }

    public void StartAnim()
    {
        m_isAnim = true;
        m_timer = 0;

        m_startPos = m_rectTransform.anchoredPosition3D;
        m_startScale = transform.localScale;
        m_startAlpha = m_text.color.a;
    }

    void Update()
    {
        if(m_isAnim)
        {
            m_timer += Time.deltaTime;
            SetAnim();

            if(m_timer > m_totalAnimTime)
            {
                m_isAnim = false;
            }
        }
    }

    void SetAnim()
    {
        m_rectTransform.anchoredPosition = CalcCurrentPos();
        transform.localScale             = CalcCurrentScale();

        SetAlpha(CalcCurrentAlpha());
    }

    void SetAlpha(float a)
    {
        Color col = m_text.color;
        col.a = a;

        m_text.color = col;
    }

    Vector3 m_itemSpace = new Vector3(0,40,0);
    Vector3 m_centerPos = new Vector3(0,-400,0);

    Vector3 m_startPos = Vector3.zero;
    Vector3 m_startScale = Vector3.zero;
    float m_startAlpha = 1;

    Vector3 CalcAimPos()
    {
        Vector3 aimPos = m_centerPos + (m_totalItemCount - m_currentIndex) * m_itemSpace;
  
        return aimPos;
    }

    Vector3 CalcCurrentPos()
    {
        Vector3 currentPos = new Vector3(0, MoveFormula(m_startPos.y, CalcAimPos().y, m_timer, m_totalAnimTime), 0);

        return currentPos;
    }

    Vector3 CalcAimScale()
    {
        int tmp = m_totalItemCount - m_showCount;

        int currentTmp = (m_currentIndex - tmp > 0 ? m_currentIndex - tmp : 0);
        int totalTmp = m_showCount;

        Vector3 currentScale = new Vector3(MoveFormula(m_minScale, 1, currentTmp, totalTmp),
            MoveFormula(m_minScale, 1, currentTmp, totalTmp),
            MoveFormula(m_minScale, 1, currentTmp, totalTmp));

        return currentScale;
    }

    Vector3 CalcCurrentScale()
    {
        Vector3 currentPos = new Vector3(Mathf.Lerp(m_startScale.x, CalcAimScale().x, m_timer / m_totalAnimTime),
            Mathf.Lerp(m_startScale.y, CalcAimScale().y, m_timer / m_totalAnimTime),
            Mathf.Lerp(m_startScale.z, CalcAimScale().z, m_timer / m_totalAnimTime));

        return currentPos;
    }

    float CalcAimAlpha()
    {
        int tmp = m_totalItemCount - m_showCount;

        int currentTmp = (m_currentIndex - tmp > 0 ? m_currentIndex - tmp : 0);
        int totalTmp = m_showCount;

        //Debug.Log((float)currentTmp / (float)totalTmp);

        return MoveFormula(m_minAlpha, 1, (float)currentTmp, (float)totalTmp);
    }

    float CalcCurrentAlpha()
    {
        return Mathf.Lerp(m_startAlpha, CalcAimAlpha(), m_timer / m_totalAnimTime);
    }

    float MoveFormula(float b, float to, float t, float d)
    {
        return OutQuart(b, to, t, d);
    }

    float AlphaFormula(float b, float to, float t, float d)
    {
        return OutCirc(b, to, t, d);
    }

    float OutQuart(float b, float to, float t, float d)
    {
        float c = to - b;
        t = t / d - 1;
        return (float)(-c * (Math.Pow(t, 4) - 1) + b);
    }

    float OutCirc(float b, float to, float t, float d)
    {
        float c = to - b;
        t = t / d - 1;
        return (float)(c * (Math.Sqrt(1 - Math.Pow(t,2)) - 1) + b);
    }
}
