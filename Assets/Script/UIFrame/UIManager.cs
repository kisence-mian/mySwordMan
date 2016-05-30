using UnityEngine;
using System.Collections;

public class UIManager : GameStaticClassBase
{
    #region 静态部分

    static UIManager instance;

    public static UIManager getInstance()
    {
        if (instance == null)
        {
            instance = (UIManager)GetInstance<UIManager>();
        }

        return instance;
    }

    public static void openUI(string UIname)
    {
        Resources.Load("" + UIname);
    }

    public static void closeUI(string UIname)
    {

    }


    #endregion 

    #region 实例部分

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    #endregion
}
