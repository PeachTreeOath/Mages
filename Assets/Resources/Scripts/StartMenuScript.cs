using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuScript : MonoBehaviour
{

	int numPlayers = 0;
	GameObject p1, p2, p3, p4, p5, p6, p7, p8, startButton;
	GlobalObject globalObj;
	private bool[] playerArr;
	private GameObject[] playerObjs;
	private GameObject playerRes;
	private bool[] dpadPressed;
	// Used to make dpad act like button

	void Start ()
	{
		playerArr = new bool[8];
		playerObjs = new GameObject[8];
		dpadPressed = new bool[4];
		p1 = GameObject.Find ("P1");
		p2 = GameObject.Find ("P2");
		p3 = GameObject.Find ("P3");
		p4 = GameObject.Find ("P4");
		p5 = GameObject.Find ("P5");
		p6 = GameObject.Find ("P6");
		p7 = GameObject.Find ("P7");
		p8 = GameObject.Find ("P8");
		playerObjs [0] = p1;
		playerObjs [1] = p2;
		playerObjs [2] = p3;
		playerObjs [3] = p4;
		playerObjs [4] = p5;
		playerObjs [5] = p6;
		playerObjs [6] = p7;
		playerObjs [7] = p8;

		startButton = GameObject.Find ("Start");
	}

	// Update is called once per frame
	void Update ()
	{
		Debug.Log((Input.GetAxisRaw ("Action_p1")));
		for (int i = 0; i < 8; i++) {
			//TODO if (i < 4) {
			if (i < 1) {
				// Only players 1-4 are allowed to solo controllers
				if (Input.GetButtonDown ("Action_p" + (i + 1) + "_solo")) {
					TogglePlayer (i);
				}
				// Players 1-4 in coop setting use dpad down to action
				else if (Input.GetAxisRaw ("Action_p" + (i + 1)) > 0) {
					if (!dpadPressed [i]) {
						TogglePlayer (i);
						dpadPressed [i] = true;
					}
				} else if (Input.GetAxisRaw ("Action_p" + (i + 1)) == 0) {
					dpadPressed [i] = false;
				}
			} else {
				// Players 5-8 are forced coop setting and use Y to action
				//if (Input.GetButtonDown ("Action_p" + (i + 1)) ){
				//	TogglePlayer (i);
				//}
			}

		}



		if (numPlayers > 0 && Input.GetButtonDown ("Submit")) {
			if (GlobalObject.instance != null) {
				GlobalObject.instance.playerList = playerArr;
			}
			SceneManager.LoadScene ("Loadout");
		}
	}

	private void TogglePlayer (int playerNum)
	{
		if (!playerArr [playerNum]) {
			// Add new player
			numPlayers++;
			startButton.GetComponent<SpriteRenderer> ().enabled = true;
			playerObjs [playerNum].GetComponent<SpriteRenderer> ().enabled = true;
			playerArr [playerNum] = true;
		} else {
			// Remove existing player
			numPlayers--;
			if (numPlayers == 0) {
				startButton.GetComponent<SpriteRenderer> ().enabled = false;
			}
			playerObjs [playerNum].GetComponent<SpriteRenderer> ().enabled = false;
			playerArr [playerNum] = false;
		}
	}

}
