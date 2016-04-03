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
        Debug.Log("Collision: Enemy and something");
        string name = col.gameObject.name;
//		Destroy (col.gameObject);
		if (name == "Player") {
			//destroy player
		}
		if (name == "Bullet(Clone)") {
            Debug.Log("Collision: Enemy and Bullet");
            Destroy (gameObject);
		}

	}
}
