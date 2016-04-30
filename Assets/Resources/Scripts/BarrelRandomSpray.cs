using UnityEngine;
using System.Collections;

public class BarrelRandomSpray : Barrel
{
	protected override void FireShot ()
	{
		float thetaInDegs = UnityEngine.Random.Range (0, 360f);
		Quaternion bulletRotation = Quaternion.Euler (0f, 0f, thetaInDegs - 90f);
		Bullet bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, bulletRotation)).GetComponent<Bullet> ();
		bullet.SetType (type);
		bullet.owner = GetComponentInParent<Player> ();
		bullet.isPassable = isPassable;
		bullet.speed = speed;
		bullet.Fire ();
	}
}
