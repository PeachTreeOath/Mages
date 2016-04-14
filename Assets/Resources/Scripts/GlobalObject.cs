using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour {

	public static GlobalObject instance;
	public bool[] playerList = new bool[8];

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
