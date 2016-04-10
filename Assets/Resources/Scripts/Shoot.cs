using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{

	public GameObject bulletPrefab;
	public GameObject tetherPrefab;
	public float radiusFromTether;
	public float speed;
	public float angularSpeed;
	public float angularDrag;
	public float shotDelay;
	public int type;
	public int ammoCount = -1;
	// -1 is infinite bullets
	public bool isPassable;
	public bool useParentVelocity;

	private bool readyToShoot = true;
	private Rigidbody2D body;

	// Use this for initialization
	void Start ()
	{
		if (useParentVelocity) {
			body = gameObject.GetComponentInParent<Rigidbody2D> ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ammoCount != 0 && readyToShoot) {
			CmdFire ();
			readyToShoot = false;
			Invoke ("ResetReadyToShoot", shotDelay);
		}
	}

	void ResetReadyToShoot ()
	{
		readyToShoot = true;
	}

	protected virtual void CmdFire ()
	{
		Bullet bullet = null;

		if (tetherPrefab != null) {
			RotatingTether tether = ((GameObject)Instantiate (tetherPrefab, transform.position, transform.rotation)).GetComponent<RotatingTether> ();
			//tether.objectToCircle = gameObject;
			tether.transform.parent = transform;

			bullet = ((GameObject)Instantiate (bulletPrefab, tether.transform.position, tether.transform.rotation)).GetComponent<Bullet> ();
			//bullet.transform.parent = tether.transform;
			//Vector3 bulletPosition = tether.transform.position;
			//bulletPosition.y = bulletPosition.y + radiusFromTether;
			//bullet.transform.position = bulletPosition;
			//bullet.transform.rotation = tether.transform.rotation;
			bullet.speed = 1;

			//currentWeapon = (Weapon)Instantiate(nextWeapon, transform.position, transform.rotation);
			//currentWeapon.transform.parent = this.transform;
		} else {
			bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation)).GetComponent<Bullet> ();
			bullet.SetSpeed (speed, angularSpeed, angularDrag);
		}

		bullet.SetType (type);
		bullet.owner = GetComponentInParent<Player> ();
		bullet.isPassable = isPassable;
        
		bullet.Fire ();

		if (useParentVelocity) {
			bullet.GetComponent<Rigidbody2D> ().velocity += body.velocity;
		}

		ammoCount--;
	}
}
