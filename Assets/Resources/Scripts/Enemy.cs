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
        Head playerHead = col.gameObject.GetComponent<Head>();
        Bullet bullet = col.gameObject.GetComponent<Bullet>();
		//if (playerHead != null) {
		//	//destroy player
		//}
		if (bullet != null && bullet.type == 0) {
            Destroy (gameObject);
		}

	}
}
