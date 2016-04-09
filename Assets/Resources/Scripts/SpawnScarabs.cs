using UnityEngine;
using System.Collections;

public class SpawnScarabs :  SpawnObjects
{
	public float moveSpeed;
	public float sinAmplitude;
	public float sinFrequency;

	protected override GameObject Spawn ()
	{
		GameObject obj = base.Spawn ();
		obj.GetComponent<FodderAI>().BeginMoving(moveSpeed, sinAmplitude, sinFrequency);

		return obj;
	}
		
}
