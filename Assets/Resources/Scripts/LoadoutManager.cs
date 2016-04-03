using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class LoadoutManager : MonoBehaviour {

	private bool shot1 = true;
	private bool shot2;
	private bool shot3;
	private bool shot4;
	private bool shot5;
	private bool shot6;
	private bool shot7;
	private bool shot8;
	private bool shot9;
	private bool eshot1;
	private bool eshot2;
	private bool eshot3;
	private bool eshot4;
	private bool eshot5;
	private bool eshot6;
	private bool eshot7;
	private bool eshot8;
	private bool eshot9;

	private int shot1Val = -1;
	private int shot2Val = -3;
	private int shot3Val = -4;
	private int shot4Val;
	private int shot5Val;
	private int shot6Val;
	private int shot7Val;
	private int shot8Val;
	private int shot9Val;
	private int eshot1Val = 3;
	private int eshot2Val = 6;
	private int eshot3Val;
	private int eshot4Val;
	private int eshot5Val;
	private int eshot6Val;
	private int eshot7Val;
	private int eshot8Val;
	private int eshot9Val;

	private int points = 0;
	private Text pointsText;
	private Text warningText;

	// Use this for initialization
	void Start () {
		pointsText = GameObject.Find ("PointsText").GetComponent<Text> ();
		warningText = GameObject.Find ("Warning").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Toggle(string name)
	{
		switch (name) {
		case "shotNormal":
			shot1 = !shot1;
			points += shot1Val * BoolToInt(shot1);
			break;
		case "shotTwin":
			shot2 = !shot2;
			points += shot2Val * BoolToInt(shot2);
			break;
		case "shotSpread":
			shot3 = !shot3;
			points += shot3Val * BoolToInt(shot3);
			break;
		case "eshotMine":
			eshot1 = !eshot1;
			points += eshot1Val * BoolToInt(eshot1);
			break;
		case "eshotBurst":
			eshot2 = !eshot2;
			points += eshot2Val * BoolToInt(eshot2);
			break;
		}

		pointsText.text = Convert.ToString(points);

	}

	private int BoolToInt(bool val)
	{
		if (val) {
			return 1;
		} else {
			return -1;
		}
	}

	public void ReadyPressed()
	{
		if (points >= 0) {
            Debug.Log("Ready pressed, loading game");
			warningText.enabled = false;
            //SceneManager.LoadScene(SceneState.getFirstScene());
            NetworkManager.singleton.ServerChangeScene(SceneState.getFirstScene());
		} else {
			warningText.enabled = true;
		}
	}
}
