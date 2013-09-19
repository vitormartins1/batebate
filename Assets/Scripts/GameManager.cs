using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GAMESTATE state;
	
	void Awake()
	{
		DontDestroyOnLoad (this);;
	}
	
	void Start ()
	{
		state = GAMESTATE.MENU;
	}
	
	void Update ()
	{
		switch (state)
		{
		case GAMESTATE.MENU:
			break;
		case GAMESTATE.INSTRUCTIONS:
			break;
		case GAMESTATE.CREDITS:
			break;
		case GAMESTATE.ONE_PLAYER:
			break;
		case GAMESTATE.TWO_PLAYERS:
			break;
		case GAMESTATE.FOUR_PLAYERS:
			break;
		case GAMESTATE.RESULT:
			break;
		}
	}
	
	public enum GAMESTATE
	{
		MENU,
		ONE_PLAYER,
		TWO_PLAYERS,
		FOUR_PLAYERS,
		RESULT,
		CREDITS,
		INSTRUCTIONS,
	}
}