using UnityEngine;
using System.Collections;

public class GameStaticClassBase : MonoBehaviour 
{
    static GameStaticClassBase instance;

    static string staticClassName = "GameStaticClass";

    public static GameStaticClassBase GetInstance<T>() where T : GameStaticClassBase
    {
        if (instance == null)
        {
            GameObject instanceGO = GameObject.Find(staticClassName);

            if (instanceGO == null)
            {
                instanceGO = new GameObject();
                instanceGO.name = staticClassName;
            }

            instance = instanceGO.AddComponent<T>();
        }

        return instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
