using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

	public enum GameStates { StartMenu, PreGame, CoreGameLoop, Scoring, Celebrating }


public struct CardData
{
	string cardName;
	int PlayerNumber;
	int placeInPlayersHand;
	int rank;
}


	public struct PlayerNames
{
	public string Player1, Player2, Player3, Player4;
  public string Color1, Color2, Color3, Color4;
}

public struct PlayersCurrentHand { public CardData Card1, Card2, Card3, Card4; }
	/// <summary>
	/// Singleton which manages game states for the overall game.
    /// </summary>
	public class GameManager : Singleton<GameManager>
	{

	//list of all the cards in case we need to know what cards didnt make it to hand.
	//this will be updated when needed from outside
	public List<CardRATC> AllTheCards;

	//list of cards in case we need to know what cards didnt make it to hand.
    //this will be updated when needed from outside
	public List<CardRATC> CardsNotInHand;

		static public GameManager GM;
		public GameStates state = GameStates.StartMenu;


	public PlayerNames allPlayerNames;
	public PlayersCurrentHand Player1Hand, Player2Hand, Player3Hand, Player4Hand;


		int numberOfRevives; //times characters been revived 
		float totalGameTime;                        //Length of the total session time
		
		private DateTime _sessionStartTime;
		private DateTime _sessionEndTime;

		bool isGameOver;                            //Is the game currently over?
		public int Scene;



        void Start()
        {
			//TODO:
			// Create Menu Scene 
			_sessionStartTime = DateTime.Now;
			Debug.Log("Game session start @: " + DateTime.Now);
		}


		void Update()
		{
			//If the game is over, exit
			if (isGameOver)
				return;

			//Update the total game time and tell the UI Manager to update
			totalGameTime += Time.deltaTime;

		}

    #region Player Name Data Control
    public void ClearPlayerNames() {
		allPlayerNames.Player1 = "";
		allPlayerNames.Player2 = "";
		allPlayerNames.Player3 = "";
		allPlayerNames.Player4 = "";
	}
	public void SetNamePlayer1(string Player1Name)
	{
		allPlayerNames.Player1 = Player1Name;
	}
	public void ClearNamePlayer1()
	{
		allPlayerNames.Player1 = "";
	}
	public void SetNamePlayer2(string Player2Name)
	{
		allPlayerNames.Player2 = Player2Name;
	}
	public void ClearNamePlayer2()
	{
		allPlayerNames.Player2 = "";
	}
	public void SetNamePlayer3(string Player3Name)
	{
		allPlayerNames.Player3 = Player3Name;
	}
	public void ClearNamePlayer3()
	{
		allPlayerNames.Player3 = "";
	}
	public void SetNamePlayer4(string Player4Name)
	{
		allPlayerNames.Player4 = Player4Name;
	}
	public void ClearNamePlayer4()
	{
		allPlayerNames.Player4 = "";
	}
    #endregion


    void OnApplicationQuit()
		{
			_sessionEndTime = DateTime.Now;
			TimeSpan timeDifference =
			_sessionEndTime.Subtract(_sessionStartTime);
			Debug.Log(
			"Game session ended @: " + DateTime.Now);
			Debug.Log(
			"Game session lasted: " + timeDifference);
		}

	/// <summary>
    /// Debugging button
    /// </summary>
		void OnGUI()
		{
			if (GUILayout.Button("Next Scene"))
			{
			ChangeScene();
			}
		}


#region Scene Control
	public void RestartScene()
		{
			//Play the scene restart audio
//			AudioManager.PlaySceneRestartAudio();

			
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

	/// <summary>
	/// Generic Change scene command
	/// </summary>
	/// <param name="state"></param>
	public void ChangeScene()
	{
		SceneManager.LoadScene(
				   SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void ChangeSceneMenu() 
		{
		state = GameStates.StartMenu;
			SceneManager.LoadScene(0);
		}
	

	public void ChangeSceneGameplay() {
		state = GameStates.PreGame;
		SceneManager.LoadScene(1);
	}
	#endregion
}
