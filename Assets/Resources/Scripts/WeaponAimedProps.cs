using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO rename this something like "aimed shot"
public class WeaponAimedProps : WeaponProps
{
	public Vector2 topLeftShotBound;
	public Vector2 bottomRightShotBound;

	public override List<GameObject> doFireCallback()
	{
        List<GameObject> objs = new List<GameObject>();
        //GameObject bulletgo = createBullet(transform.position, Quaternion.AngleAxis(clockAngle, transform.up));
        GameObject bulletgo = createBullet(transform.position, transform.rotation);
        ExplodingBullet bullet = bulletgo.GetComponent<ExplodingBullet>();

		// Determine distance to fire
		Vector2 targetLoc = ChooseNextShotLocation ();
		float distance = Vector2.Distance (transform.position, targetLoc);
		bullet.SetDistance (distance);
		Vector3 vectorToTarget = targetLoc - (Vector2)transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

        objs.Add(bulletgo);
        decrementAmmo();

        return objs;
	}


	private Vector2 ChooseNextShotLocation ()
	{	
		float x = Random.Range (topLeftShotBound.x, bottomRightShotBound.x);
		float y = Random.Range (topLeftShotBound.y, bottomRightShotBound.y);
		return new Vector2 (x, y);
	}
}
