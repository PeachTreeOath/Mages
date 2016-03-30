using UnityEngine;
using System.Collections;

public class EnemyScarab : Enemy {

	// Use this for initialization
	void Start () {
		hp = 1;
	}
	
	// Update is called once per frame
	void Update () {
		 
	}


	void OnTriggerEnter2d(Collider2D col)
	{
		Debug.Log ("TEST");
		string name = col.gameObject.name;

		if (name == "Player") {
			//destroy player
		}
		if (name == "Bullet(Clone)") {
			Destroy (gameObject);
		}

	}
}
