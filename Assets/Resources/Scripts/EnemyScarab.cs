using UnityEngine;
using System.Collections;

public class EnemyScarab : Enemy {

	// Use this for initialization
	public override void Start () {
		hp = 1;
	}
	
	// Update is called once per frame
	public override void Update () {
		 
	}


	public override void OnTriggerEnter2D(Collider2D col)
	{
        Bullet bullet = col.gameObject.GetComponent<Bullet>();
        Head playerHead = col.gameObject.GetComponent<Head>();

		if (bullet != null && bullet.type==0) {
			//TODO change to hp dmg
            Destroy(gameObject);
		}
	}
}
