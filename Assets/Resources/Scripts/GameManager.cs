using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Hack just to get single player respawns working
	public void SpawnPlayer()
	{
		Instantiate (playerPrefab, new Vector2 (0, -3.25f), Quaternion.identity);
	}
}
