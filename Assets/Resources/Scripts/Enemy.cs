using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public float hp;

	public virtual void OnTriggerEnter2D (Collider2D col)
	{
		Bullet bullet = col.gameObject.GetComponent<Bullet> ();

		if (bullet != null && bullet.type == 0) {
			hp -= bullet.damage;
			Debug.Log (hp);
			if (hp <= 0) {
				GetComponent<GibManual> ().Explode ();
			}
		}
	}
}
