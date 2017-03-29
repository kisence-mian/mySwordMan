using UnityEngine;
using System.Collections;

public class GameStateManager : GameStaticClassBase 
{
    static GameStateManager instance;
    public static GameStateManager getInstance()
    {
        if (instance == null)
        {
            instance = (GameStateManager)GetInstance<GameStateManager>();
        }

        return instance;
    }

    public StateBase currentGameState;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void Init()
    {
        currentGameState = new MainMenuState();

        currentGameState.IntoState();
    }
}
