using UnityEngine;
using System.Collections;

public class Barrel: MonoBehaviour
{

	public GameObject bulletPrefab;
	//public float radius = 1f;
	//public int numberOfBullets = 16;
	//public float angularSpeed = 0f;
	//public float angularDrag = 0f;
	public float speed = 1f;
	public int type;
	public bool isPassable = false;
	public float shotDelay = .3f;
	public float burstTime = 1f;
	public float burstDelay = 3f;
	//Setting burst delay to 0 produces continous fire
	public float perturbation = .1f;
	public float bulletLifetime = -1f;

	protected bool fireOn = false;
	protected bool readyToShoot = true;
	protected bool inBurst = false;
	protected float burstTimer;
	//private Rigidbody2D body;
	//private float preTheta;
	//private float twoPi;

	// Use this for initialization
	protected virtual void Start ()
	{
		//body = gameObject.GetComponentInParent<Rigidbody2D>();

		//preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
		//twoPi = Mathf.PI * 2f;
		burstTimer = Time.time;
	}


	// Update is called once per frame
	protected virtual void Update ()
	{
		if (!inBurst && Time.time > burstTimer + burstDelay) {
			inBurst = true;
			burstTimer = Time.time;
		}
		if (inBurst && Time.time > burstTimer + burstTime) {
			inBurst = false;
			burstTimer = Time.time;
		}
		if (fireOn && inBurst && readyToShoot) {
			FireShot ();
			readyToShoot = false;
			Invoke ("ResetReadyToShoot", shotDelay);
		}
		//transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime, Space.World);

	}

	protected virtual void ResetReadyToShoot ()
	{
		readyToShoot = true;
	}

	protected virtual void FireShot ()
	{
		Bullet bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation)).GetComponent<Bullet> ();
		bullet.SetType (type);
		bullet.owner = GetComponentInParent<Player> ();
		bullet.isPassable = isPassable;
		bullet.speed = speed;
		bullet.bulletLifetime = bulletLifetime;
		//bullet.transform.parent = transform;
		bullet.Fire ();
	}

	public virtual void FireOn ()
	{
		fireOn = true;
	}

	public virtual void FireOff ()
	{
		fireOn = false;
	}

	public virtual bool isFireOn ()
	{
		return fireOn;
	}

}
