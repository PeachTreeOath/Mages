using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{

	[SerializeField]
	private int playerNum = 0;

	private int value;
	private bool soloPlay = true;
	private int xPos;
	private int yPos;
	private bool isMoving;
	private bool dpadPressed;
	private LoadoutManager loadMgr;
	private HashSet<string> weapons;

	// Use this for initialization
	void Start ()
	{
		weapons = new HashSet<string> ();

		loadMgr = GameObject.Find ("LoadoutManager").GetComponent<LoadoutManager> ();
		if (GlobalObject.instance != null) {
			bool playerExists = GlobalObject.instance.GetComponent<GlobalObject> ().playerList [playerNum - 1];
			if (playerExists) {
				if (playerNum < 5) {
					bool coopPlayerExists = GlobalObject.instance.GetComponent<GlobalObject> ().playerList [playerNum + 3];
					if (coopPlayerExists) {
						soloPlay = false;
					}
				} else {
					soloPlay = false;
				}
			}
		} else if (playerNum > 4) {
			soloPlay = false;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		float hMove = 0;
		float vMove = 0;

		// Only players 1-4 are allowed to solo controllers
		if (soloPlay) {
			hMove = Input.GetAxis ("Horizontal_p" + playerNum + "_solo");
			vMove = Input.GetAxis ("Vertical_p" + playerNum + "_solo");
		} else {
			hMove = Input.GetAxis ("Horizontal_p" + playerNum);
			vMove = Input.GetAxis ("Vertical_p" + playerNum);
		}

		if (hMove > 0 && !isMoving) {
			if (xPos + 1 < loadMgr.cols) {
				xPos++;
				transform.position += new Vector3 (2, 0, 0);
			}
			isMoving = true;
		} else if (hMove < 0 && !isMoving) {
			if (xPos - 1 >= 0) {
				xPos--;
				transform.position += new Vector3 (-2, 0, 0);
			}
			isMoving = true;
		} 

		if (vMove > 0 && !isMoving) {
			if (yPos - 1 >= 0) {
				yPos--;
				transform.position += new Vector3 (0, 2.5f, 0);
			}
			isMoving = true;
		} else if (vMove < 0 && !isMoving) {
			if (yPos + 1 < loadMgr.rows) {
				yPos++;
				transform.position += new Vector3 (0, -2.5f, 0);
			}
			isMoving = true;
		} 

		if (hMove == 0 && vMove == 0) {
			isMoving = false;
		}
	
		if (soloPlay) {
			if (Input.GetButtonDown ("Action_p" + playerNum + "_solo")) {
				ToggleWeapon ();
			}
		} else {
			if (playerNum < 4) {
				// Players 1-4 in coop setting use dpad down to action
				if (Input.GetAxisRaw ("Action_p" + playerNum) > 0) {
					if (!dpadPressed) {
						ToggleWeapon ();
						dpadPressed = true;
					}
				} else if (Input.GetAxisRaw ("Action_p" + playerNum) == 0) {
					dpadPressed = false;
				}
			} else {
				// Players 5-8 are forced coop setting and use Y to action
				if (Input.GetButtonDown ("Action_p" + playerNum)) {
					ToggleWeapon ();
				}
			}

		}
	}

	private void ToggleWeapon ()
	{
		LoadoutToggler weapon = loadMgr.GetWeapon (yPos, xPos);
		string wepName = weapon.name;
		if (weapons.Contains (wepName)) {
			weapons.Remove (wepName);
		} else {
			weapons.Add (wepName);
		}

		SpriteRenderer tick = weapon.transform.Find ("tick" + playerNum).GetComponent<SpriteRenderer> ();
		tick.enabled = !tick.enabled;
	}
}
