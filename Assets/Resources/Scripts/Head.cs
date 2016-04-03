using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		string name = col.gameObject.name;
        if (name == "Bullet(Clone)" ||
            name == "Scarab(Clone)") {
			Die ();
		}
	}

	public void Die()
	{
		GetComponentInParent<Player> ().Die ();
	}
}
