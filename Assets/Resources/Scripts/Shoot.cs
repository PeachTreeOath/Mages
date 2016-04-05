using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bulletPrefab;
    public GameObject tetherPrefab;
    public float radiusFromTether;
	public float speed;
	public float shotDelay;
	public int type;
    public bool isPassable = false;

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
        if (tetherPrefab != null)
        {
            RotatingTether tether = ((GameObject)Instantiate(tetherPrefab, transform.position, transform.rotation)).GetComponent<RotatingTether>();
            //tether.objectToCircle = gameObject;
            tether.transform.parent = transform;

            bullet.transform.parent = tether.transform;
            //Vector3 bulletPosition = tether.transform.position;
            //bulletPosition.y = bulletPosition.y + radiusFromTether;
            //bullet.transform.position = bulletPosition;
            //bullet.transform.rotation = tether.transform.rotation;
            //bullet.speed = 0;

            //currentWeapon = (Weapon)Instantiate(nextWeapon, transform.position, transform.rotation);
            //currentWeapon.transform.parent = this.transform;
        }
        else
        {
            bullet.SetSpeed(speed);
        }

        bullet.SetType (type);
        bullet.owner = GetComponentInParent<Player>();
        bullet.isPassable = isPassable;
        
		bullet.Fire ();
	}
}
