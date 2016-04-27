using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class LoadoutManager : MonoBehaviour
{

	public int rows;
	public int cols;
	public LoadoutToggler[,] weaponMap;

	private int points = 0;
	//	private Text pointsText;
	private Text warningText;
	public bool[] playerList = new bool[8];

	// Use this for initialization
	void Start ()
	{
		GlobalObject global = GlobalObject.instance;
		if (global == null) {
			playerList [0] = true;
		} else {
			playerList = global.playerList;
		}
		ShowPlayerUI ();

		weaponMap = new LoadoutToggler[rows, cols];
		weaponMap [0, 0] = GameObject.Find ("shotNormal").GetComponent<LoadoutToggler> ();
		weaponMap [0, 1] = GameObject.Find ("shotTwin").GetComponent<LoadoutToggler> ();
		weaponMap [0, 2] = GameObject.Find ("shotSpread").GetComponent<LoadoutToggler> ();
		weaponMap [0, 3] = GameObject.Find ("shotBig").GetComponent<LoadoutToggler> ();
		weaponMap [0, 4] = GameObject.Find ("eshotMine").GetComponent<LoadoutToggler> ();
		weaponMap [0, 5] = GameObject.Find ("eshotBurst").GetComponent<LoadoutToggler> ();

		//pointsText = GameObject.Find ("PointsText").GetComponent<Text> ();
		warningText = GameObject.Find ("Warning").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void ShowPlayerUI ()
	{
		for (int i = 0; i < 8; i++) {
			if (playerList [0]) {
				GameObject p = GameObject.Find ("P" + (i + 1));
				p.GetComponent<Text> ().enabled = true;
				p.transform.Find ("value").GetComponent<Text> ().enabled = true;
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
		if (points >= 0) {
			Debug.Log ("Ready pressed, loading game");
			warningText.enabled = false;
			SceneManager.LoadScene ("Game");
		} else {
			warningText.enabled = true;
		}
	}

	public LoadoutToggler GetWeapon (int row, int col)
	{
		return weaponMap [row, col];
	}
}
