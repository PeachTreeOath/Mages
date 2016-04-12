using UnityEngine;
using System.Collections;

public class ShootSnake : Shoot
{
	public Vector2 topLeftShotBound;
	public Vector2 bottomRightShotBound;

	protected override void CmdFire ()
	{
		BulletSnake bullet = ((GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation)).GetComponent<BulletSnake> ();
		bullet.SetSpeed (speed, angularSpeed, angularDrag);
		bullet.SetType (type);
		bullet.owner = GetComponentInParent<Player> ();
		bullet.isPassable = isPassable;

		// Determine distance to fire
		Vector2 targetLoc = ChooseNextShotLocation ();
		float distance = Vector2.Distance (transform.position, targetLoc);
		bullet.SetDistance (distance);
		Vector3 vectorToTarget = targetLoc - (Vector2)transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		bullet.Fire ();

		ammoCount--;
	}

	public void RegularFire()
	{
		base.CmdFire ();
	}

	private Vector2 ChooseNextShotLocation ()
	{	
		float x = Random.Range (topLeftShotBound.x, bottomRightShotBound.x);
		float y = Random.Range (topLeftShotBound.y, bottomRightShotBound.y);
		return new Vector2 (x, y);
	}
}
