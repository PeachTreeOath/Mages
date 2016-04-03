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
        Bullet bullet = col.gameObject.GetComponent<Bullet>();
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if ((bullet != null && bullet.type >= 1) ||
            enemy != null) {
			Die ();
		}
	}

	public void Die()
	{
		GetComponentInParent<Player> ().Die ();
	}
}
