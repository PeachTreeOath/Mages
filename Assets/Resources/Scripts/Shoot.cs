using UnityEngine;
using System.Collections;

public class Shoot : Barrel
{

	//public GameObject bulletPrefab;
	//public GameObject tetherPrefab;
	//public float radiusFromTether;
	//public float speed;
	public float angularSpeed;
	public float angularDrag;
	//public float shotDelay;
	//public int type;
	public int ammoCount = -1;
	// -1 is infinite bullets
	//public bool isPassable;
	public bool useParentVelocity;

	//private bool readyToShoot = true;
	private Rigidbody2D body;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		if (useParentVelocity) {
			body = gameObject.GetComponentInParent<Rigidbody2D> ();
		}
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		if (ammoCount != 0 && readyToShoot) {
			FireShot ();
			readyToShoot = false;
			Invoke ("ResetReadyToShoot", shotDelay);
		}
	}

	protected override void FireShot ()
	{
		Bullet bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation)).GetComponent<Bullet> ();
		bullet.SetSpeed (speed, angularSpeed, angularDrag);
		bullet.SetType (type);
		bullet.owner = GetComponentInParent<Player> ();
		bullet.isPassable = isPassable;
		bullet.bulletLifetime = bulletLifetime;
		bullet.Fire ();

		if (useParentVelocity) {
			bullet.GetComponent<Rigidbody2D> ().velocity += body.velocity;
		}

		ammoCount--;
	}
}
