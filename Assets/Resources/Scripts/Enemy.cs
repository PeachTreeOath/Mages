using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float hp;

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}
		
	public virtual void OnTriggerEnter2D(Collider2D col)
	{
		Bullet bullet = col.gameObject.GetComponent<Bullet>();

		if (bullet != null && bullet.type==0) {
			//TODO change to hp dmg
			Destroy(gameObject);
		}
	}
}
