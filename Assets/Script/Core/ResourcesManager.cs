using UnityEngine;
using System.Collections;

public class ResourcesManager : GameStaticClassBase 
{
    static ResourcesManager instance;

    public static ResourcesManager GetInstance()
    {
        if(instance == null)
        {
            instance = (ResourcesManager)GetInstance<ResourcesManager>();
        }

        return instance;
    }

    public static GameObject LoadGameObject(string path)
    {
        return GetInstance().loadGameobject(path);
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public GameObject loadGameobject(string path)
    {
        return (GameObject)Resources.Load(path);
    }
}
