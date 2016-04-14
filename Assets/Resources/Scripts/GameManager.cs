using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public GameObject playerPrefab;

	private bool[] playerList = new bool[8];

	// Use this for initialization
	void Start ()
	{
		if (GlobalObject.instance != null) {
			playerList = GlobalObject.instance.playerList;
		}

		//TODO: THIS IS ONLY FOR TESTING
		playerList [0] = true;
		playerList [1] = true;

		CreatePlayers ();
	}
		
	// Update is called once per frame
	void Update ()
	{
	
	}

	// NOTE: This doesn't protect against players 5-8 solo playing, because you're a fool if you decide to only play with half a controller solo
	private void CreatePlayers ()
	{
		if (playerList [0]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 1;

			if (playerList [4]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 5;
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [1]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 2;

			if (playerList [5]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 6;
			} else {
				player.soloPlay = true;
			}
		}

		//TODO Add support for rest of players
	}
}
