using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalObject : MonoBehaviour
{

	public static GlobalObject instance;

	/**
	 *	0: Stage 1
	 *	1: Boss 1
	 *	2: Stage 2
	 *	3: Boss 2
	 * 	4: Stage 3
	 *	5: Boss 3
	 **/
	public int section;
	public bool[] playerList = new bool[8];
	public Dictionary<int,HashSet<string>> weaponMap = new Dictionary<int, HashSet<string>> ();
	public bool easyModeOn;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
