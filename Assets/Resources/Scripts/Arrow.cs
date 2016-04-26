using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{

	[SerializeField]
	private int playerNum = 0;

	private bool soloPlay = true;
	private int xPos;
	private int yPos;
	private bool isMoving;
	private LoadoutManager loadMgr;
	// Use this for initialization
	void Start ()
	{
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
		// Only players 1-4 are allowed to solo controllers
		if (soloPlay) {
			float hMove = Input.GetAxis ("Horizontal_p" + playerNum + "_solo");
			float vMove = Input.GetAxis ("Vertical_p" + playerNum + "_solo");
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
			
		} else {
			if (playerNum < 4) {
				// Players 1-4 in coop setting use dpad down to action
				if (Input.GetAxisRaw ("Action_p" + playerNum) > 0) {
					//currSpeed = speed * 0.33f;
				}
			} else {
				// Players 5-8 are forced coop setting and use Y to action
				if (Input.GetButton ("Action_p" + playerNum)) {
					//	currSpeed = speed * 0.33f;
				}
			}
			//transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal_p" + playerNum) * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical_p" + playerNum) * Time.deltaTime * currSpeed);
		}
	}
}
