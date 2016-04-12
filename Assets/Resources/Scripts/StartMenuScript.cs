using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuScript : MonoBehaviour
{

	int numPlayers = 0;
	GameObject p1, p2, p3, p4, p5, p6, p7, p8, startButton;
	GlobalObject globalObj;
	private bool[] playerArr;
	private GameObject playerRes;

	void Start ()
	{
		playerArr = new bool[8];

		p1 = GameObject.Find ("P1");
		p2 = GameObject.Find ("P2");
		p3 = GameObject.Find ("P3");
		p4 = GameObject.Find ("P4");
		p5 = GameObject.Find ("P5");
		p6 = GameObject.Find ("P6");
		p7 = GameObject.Find ("P7");
		p8 = GameObject.Find ("P8");
		startButton = GameObject.Find ("Start");

		globalObj = GameObject.Find ("GlobalObject").GetComponent<GlobalObject> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Action_p1") || Input.GetButtonDown ("Action_p1_solo")) {
			if (!playerArr [0]) {
				AddPlayer (1);
				playerArr [0] = true;
			}
		}
			/*
		if (Input.GetButtonDown ("Jump_p2")) {
			if (!playerArr [1]) {
				AddPlayer (2);
				playerArr [1] = true;
			}
		}
		if (Input.GetButtonDown ("Jump_p3")) {
			if (!playerArr [2]) {
				AddPlayer (3);
				playerArr [2] = true;
			}
		}
		if (Input.GetButtonDown ("Jump_p4")) {
			if (!playerArr [3]) {
				AddPlayer (4);
				playerArr [3] = true;
			}
		}
*/

		if (numPlayers > 0 && Input.GetButtonDown ("Submit")) {
			globalObj.playerList = playerArr;
			SceneManager.LoadScene ("Loadout");
		}
	}

	private void AddPlayer (int playerNum)
	{
		startButton.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/PressStart");

		numPlayers += 1;

		/*
		switch (numPlayers) {
		case 1:
			globalObj.p1JoyMap = "_p" + playerNum;
			break;
		case 2:
			globalObj.p2JoyMap = "_p" + playerNum;
			break;
		case 3:
			globalObj.p3JoyMap = "_p" + playerNum;
			break;
		case 4:
			globalObj.p4JoyMap = "_p" + playerNum;
			break;
		}

		switch (numPlayers) {
		case 1:
			q1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj1 = ((GameObject)Instantiate (playerRes, q1.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj1.numPlayer = numPlayers - 1;
			playerObj1.transform.localScale = new Vector2 (2, 2);
			//q1.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 2:
			q2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj2 = ((GameObject)Instantiate (playerRes, q2.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj2.numPlayer = numPlayers - 1;
			playerObj2.transform.localScale = new Vector2 (2, 2);
			//q2.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 3:
			q3.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj3 = ((GameObject)Instantiate (playerRes, q3.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj3.numPlayer = numPlayers - 1;
			playerObj3.transform.localScale = new Vector2 (2, 2);
			//q3.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		case 4:
			q4.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("None");
			SelectPlayer playerObj4 = ((GameObject)Instantiate (playerRes, q4.transform.position, Quaternion.identity)).GetComponent<SelectPlayer> ();
			playerObj4.numPlayer = numPlayers - 1;
			playerObj4.transform.localScale = new Vector2 (2, 2);
			//q4.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load <RuntimeAnimatorController> ("Images/Kiwi/KiwiDanceController");
			break;
		}
		*/
	}
}
