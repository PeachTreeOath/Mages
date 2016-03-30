using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float hp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		string name = col.gameObject.name;
		Destroy (col.gameObject);
		if (name == "Player") {
			//destroy player
		}
		if (name == "Bullet(Clone)") {
			Destroy (gameObject);
		}

	}
}
