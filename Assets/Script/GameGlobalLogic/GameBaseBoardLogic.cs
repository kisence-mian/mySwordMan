using UnityEngine;
using System.Collections;

public class GameBaseBoardLogic : IApplicationGlobalLogic
{
    GameBaseBoardWindow s_baseBoard;

    public override void Init()
    {
        
    }

    public void OpenBoard()
    {
        s_baseBoard = UIManager.OpenUIWindow<GameBaseBoardWindow>();
    }

    public Vector3 GetPos(int index)
    {
        return s_baseBoard.m_rsr.GetItemAnchorPos(index);
    }

    public ReusingScrollItemBase GetItem(int index)
    {
        return s_baseBoard.m_rsr.GetItem(index);
    }

    public int currentPage = 0;
    public void NextPage()
    {
        currentPage -= 1;

        Vector3 nextPos = new Vector3(0, 960, 0) * (currentPage);

        //AnimSystem.UguiMove(s_baseBoard.m_rsr.content.gameObject, nextPos, 0.5f);

        AnimSystem.CustomMethodToVector3(s_baseBoard.m_rsr.SetPos,
            s_baseBoard.m_rsr.content.anchoredPosition3D,
            nextPos, 
            1f);
    }

    public void LastPage()
    {
        currentPage -= 1;

        Vector3 lastPos = new Vector3(0, 1056, 0) * (currentPage); ;

        AnimSystem.UguiMove(s_baseBoard.m_rsr.content.gameObject,null, lastPos, 0.5f);
    }

    public void SetPress(int index,bool status)
    {
        if(GetItem(index) == null)
        {
            return;
        }

        AnimSystem.StopAnim(GetItem(index).gameObject);

        if (status)
        {
            AnimSystem.UguiColor(GetItem(index).gameObject, Color.white, Color.yellow, 0.5f);
        }
        else
        {
            AnimSystem.UguiColor(GetItem(index).gameObject, Color.yellow, Color.white, 0.5f);
        }
    }


}
