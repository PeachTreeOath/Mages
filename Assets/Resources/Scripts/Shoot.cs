using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bulletPrefab;
	public float speed;
	public float shotDelay;
	public int type;
    public bool isPassable;

	private bool readyToShoot = true;

	// Use this for initialization
	void Start () {
	
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
        bullet.isPassable = isPassable;
		bullet.Fire ();
	}
}
