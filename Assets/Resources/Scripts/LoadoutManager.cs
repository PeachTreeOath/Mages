using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LoadoutManager : MonoBehaviour
{
	public int rows;
	public int cols;
	public LoadoutWeapon[,] weaponMap;

	private Text warningText;
	public bool[] playerList = new bool[8];
	private bool[] readyList = new bool[8];

	// Use this for initialization
	void Start ()
	{
		if (GlobalObject.instance == null) {
			playerList [0] = true;
		} else {
			playerList = GlobalObject.instance.playerList;
		}
		ShowPlayerUI ();

		weaponMap = new LoadoutWeapon[rows, cols];
		weaponMap [0, 0] = GameObject.Find ("shotNormal").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 1] = GameObject.Find ("shotTwin").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 2] = GameObject.Find ("shotSpread").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 3] = GameObject.Find ("shotBig").GetComponent<LoadoutWeapon> ();
		weaponMap [1, 0] = GameObject.Find ("shotAegis").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 4] = GameObject.Find ("eshotMine").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 5] = GameObject.Find ("eshotBurst").GetComponent<LoadoutWeapon> ();
		weaponMap [0, 6] = GameObject.Find ("eshotFirestick").GetComponent<LoadoutWeapon> ();
		weaponMap [1, 6] = GameObject.Find ("eshotWheel").GetComponent<LoadoutWeapon> ();

		warningText = GameObject.Find ("Warning").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Submit")) {
			ReadyPressed ();
		}
	}

	private void ShowPlayerUI ()
	{
		GameObject shotNormal = GameObject.Find ("shotNormal");
		for (int i = 0; i < 8; i++) {
			if (playerList [i]) {
				GameObject p = GameObject.Find ("P" + (i + 1));
				p.GetComponent<Text> ().enabled = true;
				p.transform.Find ("value").GetComponent<Text> ().enabled = true;
				shotNormal.transform.Find ("tick" + (i + 1)).GetComponent<SpriteRenderer> ().enabled = true;
				GameObject.Find ("Arrow" + (i + 1)).GetComponent<SpriteRenderer> ().enabled = true;
			}
		}
	}

	private int BoolToInt (bool val)
	{
		if (val) {
			return 1;
		} else {
			return -1;
		}
	}

	public void ReadyPressed ()
	{
		// Check if all players ready
		for (int i = 0; i < 8; i++) {
			if (playerList [i]) {
				if (!readyList [i]) {
					warningText.enabled = true;
					return;
				}
			}
		}

		Debug.Log ("Ready pressed, loading game");
		warningText.enabled = false;
		SceneManager.LoadScene ("Game");
	}

	public LoadoutWeapon GetWeapon (int row, int col)
	{
		return weaponMap [row, col];
	}

	public void ReadyUp (int player, bool ready, HashSet<string> weapons)
	{
		readyList [player - 1] = ready;
		if (ready && GlobalObject.instance != null) {
			GlobalObject.instance.weaponMap [player - 1] = weapons;
		}
	}
}
