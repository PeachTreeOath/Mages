using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed;
	public float shotDelay;
	public int type;
    public bool isPassable;
	public bool useParentVelocity;

	private bool readyToShoot = true;
	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		if (useParentVelocity) {
			body = gameObject.GetComponentInParent<Rigidbody2D> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (readyToShoot) {
			CmdFire ();
			readyToShoot = false;
			Invoke ("ResetReadyToShoot", shotDelay);
		}
	}

	void ResetReadyToShoot()
	{
		readyToShoot = true;
	}

	private void CmdFire()
	{
		Bullet bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation)).GetComponent<Bullet>();
		bullet.SetType (type);
		bullet.SetSpeed (speed);
        bullet.owner = GetComponentInParent<Player>();
        bullet.isPassable = isPassable;
		bullet.Fire ();

		if (useParentVelocity) {
			bullet.GetComponent<Rigidbody2D> ().velocity +=body.velocity;
		}
	}
}
