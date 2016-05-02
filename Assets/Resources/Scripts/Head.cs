using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		Bullet bullet = col.gameObject.GetComponent<Bullet> ();
		Enemy enemy = col.gameObject.GetComponent<Enemy> ();
		Boss boss = col.gameObject.GetComponent<Boss> ();

		if (bullet != null) {
			if (bullet.type == 1) {
				//Enemy bullets.
				Die ();
			} else if (bullet.type == 2) {
				if (GetComponentInParent<Player> ().Equals (bullet.owner)) {
					// Die to own bullets after grace period ends
					if (Time.time > bullet.fireTime + bullet.ffGracePeriod) {
						Die ();
					}
				} else {
					// Die to other player's negative bullets
					Die ();
				}     
			}
		}
		if (enemy != null || boss != null) {
			//We ran into an enemy.
			Die ();
		}
        
	}

	public void Die ()
	{
		GetComponent<GibManual> ().Explode ();
		GetComponentInParent<Player> ().Die ();
	}
}
