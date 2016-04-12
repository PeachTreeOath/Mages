using UnityEngine;
using System.Collections;

public class ExplodingBullet : Bullet {

	private float distanceNeeded;
	private Vector2 origPos;
	private ShootSnake[] barrels;

    //ALL TODO
	//public override void Start () {
	//	base.Start ();
	//	origPos = transform.position;
	//	barrels = GetComponentsInChildren<ShootSnake> ();
	//}

	//// Update is called once per frame
	//public override void Update () {
	//	base.Update ();

	//	if (Vector2.Distance (origPos, transform.position) > distanceNeeded) {
	//		Detonate ();
	//	}
	//}

	public void SetDistance(float setDistance)
	{
		//distanceNeeded = setDistance;
	}

	private void Detonate()
	{
	//	foreach (ShootSnake barrel in barrels) {
	//		if (barrel.gameObject.GetInstanceID() != GetInstanceID()) {
	//			barrel.enabled = true;
	//			barrel.RegularFire ();
	//		}
	//	}
	//	Destroy (gameObject);
	}
}
