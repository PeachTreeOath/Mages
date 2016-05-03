using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

	public float speed;
	public float angularVelocity;
	public float angularDrag;
	//Whether the bullet keeps going after it collides or not.
	public bool isPassable;
	public int type;
	public int damage = 1;
	public Player owner;
	//So you don't get hit with your own bullets asap
	public float ffGracePeriod = 1f;
	public float fireTime;
	//bulletLifetime of -1 means infinite
	public float bulletLifetime = -1;
	protected Rigidbody2D body;

	// Use this for initialization
	public virtual void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	public virtual void Update ()
	{
		if (bulletLifetime != -1) {
			bulletLifetime -= Time.deltaTime;
			if (bulletLifetime <= 0) {
				Destroy (gameObject);
			}
		}
		if (body.velocity.magnitude > 0.001f) { //accounting for floating point impression.
			transform.up = body.velocity;
		}
		if (angularVelocity != 0) {
			transform.Rotate (0f, 0f, Time.deltaTime * angularVelocity);
			body.velocity = transform.up * speed;
			angularVelocity = Mathf.Clamp (angularVelocity - (Time.deltaTime * angularDrag), 0, Mathf.Infinity);
		}
	}

	void OnDestroy ()
	{


	}

	public void SetType (int newType)
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		switch (newType) {
		case 0:
			sprite.material = Resources.Load<Material> ("Materials/blueMat");
			gameObject.layer = LayerMask.NameToLayer ("PlayerBullets");
			type = 0;
			break;
		case 1:
			sprite.material = Resources.Load<Material> ("Materials/redMat");
			gameObject.layer = LayerMask.NameToLayer ("EnemyBullets");
			type = 1;
			break;
		case 2:
			sprite.material = Resources.Load<Material> ("Materials/orangeMat");
			gameObject.layer = LayerMask.NameToLayer ("EnemyBullets");
			type = 2;
			break;
		case 3:
			gameObject.layer = LayerMask.NameToLayer ("EnemyBullets");
			type = 3;
			break;
		}
	}

	public void SetSpeed (float newSpeed, float newAngularVel, float newAngularDrag)
	{
		speed = newSpeed;
		angularVelocity = newAngularVel;
		angularDrag = newAngularDrag;
	}

	public void Fire ()
	{
		body = GetComponent<Rigidbody2D> ();
		body.velocity = transform.up * speed;
		body.angularVelocity = angularVelocity;
		body.angularDrag = angularDrag;
		fireTime = Time.time;
	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		if (!isPassable) {
			if (type == 2) {
				Head playerHead = col.gameObject.GetComponent<Head> ();
				if (playerHead != null)
				if (owner.Equals (col.gameObject.GetComponentInParent<Player> ())) {
					//This is a collision between a player and its own negative bullets.
					//We do not want to destroy the bullet in this case.
					if (Time.time > fireTime + ffGracePeriod) {
						Destroy (gameObject);
					}
				} else {
					//Collided with another player, go ahead and destroy the bullet in this case."
					Destroy (gameObject);
				}
			} else { 
				//Destroy on collisions with most things.
				Destroy (gameObject);
			}
		}
	}
}
