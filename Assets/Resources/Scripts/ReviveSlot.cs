using UnityEngine;
using System.Collections;

public class ReviveSlot : MonoBehaviour
{

	public GameObject player;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void RemovePlayer ()
	{
		player = null;
	}

	public void KillPlayer ()
	{
		if (player != null) {
			player.GetComponent<Player> ().ForceDie ();
		}
	}
}
