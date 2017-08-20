using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class ImportPoemTool : EditorWindow
{
    const string c_dataName = "poemData";
    DataTable data = null;

    [MenuItem("Tools/诗词导入编辑器")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ImportPoemTool));
    }

    void OnEnable()
    {
        GameOptionService.Init();

        EditorGUIStyleData.Init();
        data = DataManager.GetData(c_dataName);

        m_poemTypes.Clear();
        m_difficultyLevels.Clear();
        DataTable DifficultyData = DataManager.GetData("DifficultyData") ;
        DataTable poemTypesData = DataManager.GetData("PoemTypeData");

        for (int i = 0; i < poemTypesData.TableIDs.Count; i++)
        {
            m_poemTypes.Add(poemTypesData.TableIDs[i]);
        }

        for (int i = 0; i < DifficultyData.TableIDs.Count; i++)
        {
            m_difficultyLevels.Add(DifficultyData.TableIDs[i]);
        }

    }
    int m_poemType;
    int m_defaultLevel;

    int m_index = 0;
    string content = "";
    string poemName = "";
    string author = "";
    string description = "";

    List<string> poemContent = new List<string>();

    List<string> m_poemTypes;
    List<string> m_difficultyLevels;

    #region 读取设置和数据
    int GetIndex()
    {
        for (int i = 1; i < 500; i++)
        {
            if (!ExistKey(m_poemType, m_defaultLevel, i))
            {
                return i;
            }
        }

        return -1000;
    }

    bool ExistKey(int poemType, int defaultLevel, int index)
    {
        string key = GetKey(poemType, defaultLevel, index);

        for (int i = 0; i < data.TableIDs.Count; i++)
        {
            if (data.TableIDs[i] == key)
            {
                return true;
            }
        }

        return false;
    }

    string GetKey(int poemType, int defaultLevel, int index)
    {
        return m_poemTypes[poemType] + "_" + m_difficultyLevels[defaultLevel] + "_" + index.ToString("D2");
    }

    bool GetIsExist(string poemName,string author)
    {
        foreach (var item in data)
        {
            if(item.Value.GetString("poemName") == poemName &&
                item.Value.GetString("author") == author
                )
            {
                return true;
            }
        }


        return false;
    }
    #endregion

    #region GUI

    Vector2 InputPos = Vector2.zero;
    bool isAutoReadByCLipBorad = false;
    string oldContent = "";

    private void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        titleContent.text = "诗词导入编辑器 ";

        m_poemType = EditorGUILayout.Popup("詩詞類型",m_poemType, m_poemTypes.ToArray());
        m_defaultLevel = EditorGUILayout.Popup("难度",m_defaultLevel, m_difficultyLevels.ToArray());

        m_index = GetIndex();

        EditorGUILayout.LabelField("序号",m_index.ToString("D2"));

        EditorGUILayout.Space();

        isAutoReadByCLipBorad = EditorGUILayout.Toggle("自动从剪贴板读取", isAutoReadByCLipBorad);

        EditorGUILayout.LabelField("导入内容:");
        InputPos = EditorGUILayout.BeginScrollView(InputPos,GUILayout.ExpandHeight(false));
        GUI.SetNextControlName("inputContent");
        string NewContent = EditorGUILayout.TextArea(content);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        if(isAutoReadByCLipBorad)
        {
            NewContent = GUIUtility.systemCopyBuffer;
            
            if(oldContent != NewContent)
            {
                //GUI.FocusControl("button");
                content = NewContent;
                preview();
            }
        }
        else
        {
            if (NewContent != "" && NewContent != content)
            {
                content = NewContent;
                preview();
            }
        }

        previewGUI();

        if (!isAutoReadByCLipBorad)
        {
            if (GUILayout.Button("从剪贴板读取"))
            {
                content = GUIUtility.systemCopyBuffer;
            }
        }

        if(!GetIsExist(poemName,author))
        {
            GUI.SetNextControlName("button");
            if (GUILayout.Button("导入"))
            {
                Import();
            }
        }
        else
        {
            GUILayout.Label("该诗已存在！");
        }

    }

    Vector2 pos = Vector2.zero;

    void previewGUI()
    {
        pos = EditorGUILayout.BeginScrollView(pos);

        poemName = EditorGUILayout.TextField("诗名", poemName);
        author = EditorGUILayout.TextField("作者", author);
        description = EditorGUILayout.TextField("描述", description);

        for (int i = 0; i < poemContent.Count; i++)
        {
            poemContent[i] = EditorGUILayout.TextField(poemContent[i]);
        }

        EditorGUILayout.EndScrollView();
    }

#endregion

    void preview()
    {
        if(m_poemTypes[m_poemType] == "tangshi")
        {
            previewTangShi();
        }
    }

    Regex regTangshiNameAndAUthor = new Regex(@"\d+([^：]+)：(\S+)");
    Regex regTangshiContent = new Regex(@"(\S+)[，。？！](\S+)[，。？！]");
    Regex regTangshiSingleContent = new Regex(@"(\S+)[，。？！]");
    void previewTangShi()
    {
        poemContent.Clear();
        string[] tmp = content.Split('\n');

        for (int i = 0; i < tmp.Length; i++)
        {
            if(regTangshiNameAndAUthor.IsMatch(tmp[i]))
            {
                Match match = regTangshiNameAndAUthor.Match(tmp[i]);
                author = match.Groups[1].Value;
                poemName = match.Groups[2].Value;
            }
            else if(regTangshiContent.IsMatch(tmp[i]))
            {
                Match match = regTangshiContent.Match(tmp[i]);
                poemContent.Add( match.Groups[1].Value);
                poemContent.Add(match.Groups[2].Value);
            }

            else if (regTangshiSingleContent.IsMatch(tmp[i]))
            {
                Match match = regTangshiSingleContent.Match(tmp[i]);
                poemContent.Add(match.Groups[1].Value);
                //poemContent.Add(match.Groups[2].Value);
            }
        }
    }


    void Import()
    {
        string key = GetKey(m_poemType, m_defaultLevel, m_index);

        SingleData dt = new SingleData();
        dt.Add("poemName", poemName);
        dt.Add("author", author);
        dt.Add("description", description);
        dt.Add("poemID", key);

        string contentTmp = "";

        for (int i = 0; i < poemContent.Count; i++)
        {
            contentTmp += poemContent[i];

            if(i != poemContent.Count -1)
            {
                contentTmp += "|";
            }
        }

        dt.Add("content", contentTmp);

        
        data.AddData(dt);

        DataEditorWindow.SaveData(c_dataName, data);

        oldContent = content;
        content = "";
        poemName = "";
        author = "";
        description = "";
        poemContent.Clear();

        GUI.FocusControl("button");
        //GUI.FocusControl("inputContent");
    }
}
