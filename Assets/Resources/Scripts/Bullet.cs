using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float speed;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetType(int newType)
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		switch (newType) {
		case 0:
			sprite.material = Resources.Load<Material> ("Materials/blueMat");
			break;
		case 1:
			sprite.material = Resources.Load<Material> ("Materials/redMat");
			break;
		case 2:
			sprite.material = Resources.Load<Material> ("Materials/yellowMat");
			break;
		}
	}
	public void Fire()
	{
		GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
	}
}
