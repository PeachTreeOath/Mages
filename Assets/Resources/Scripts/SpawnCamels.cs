using UnityEngine;
using System.Collections;

public class SpawnCamels : SpawnObjects
{
	public bool isFacingRight;
	public Vector2 moveSpeed;
	public float weaponRotateSpeed;

	protected override GameObject Spawn ()
	{
		GameObject obj = base.Spawn ();
		obj.GetComponent<EnemyCamel>().BeginMoving(isFacingRight, moveSpeed, weaponRotateSpeed);

		return obj;
	}


}
