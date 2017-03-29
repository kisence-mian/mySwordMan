using UnityEngine;
using System.Collections;

public class StateBase : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //进入状态
    public virtual void IntoState()
    {

    }

    //退出状态
    public virtual void ExitState()
    {

    }

    //状态执行
    public virtual void UpdateRun()
    {

    }
}
