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
        Debug.Log("Collision: Scarab and something");
        string name = col.gameObject.name;

		if (name == "Player") {
			col.gameObject.GetComponent<Head> ().Die ();
		}
		if (name == "Bullet(Clone)") {
            Debug.Log("Collision: Scarab and Bullet");
            Bullet bullet = col.gameObject.GetComponent<Bullet>();
            if (!bullet.isPassable)
            {
                Debug.Log("Bullet is not passable.");
                Destroy(gameObject);
            }
		}
	}
}
