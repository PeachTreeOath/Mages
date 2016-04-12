using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodingBullet : Bullet {

	private float distanceNeeded;
	private Vector2 origPos;
    //private ShootSnake[] barrels;
    [SerializeField]
    private List<WeaponProps> shrapnelBarrels; //all fire on explosion

    //ALL TODO
	void Start () {
		origPos = transform.position;
		//barrels = GetComponentsInChildren<ShootSnake> (); //not sure of the best approach here
	}

	// Update is called once per frame
	void Update () {
		if (Vector2.Distance (origPos, transform.position) > distanceNeeded) {
			Detonate ();
		}
	}

	public void SetDistance(float setDistance)
	{
		distanceNeeded = setDistance;
	}

	private void Detonate()
	{
		foreach (WeaponProps barrel in shrapnelBarrels) {
			//if (barrel.gameObject.GetInstanceID() != GetInstanceID()) {
				barrel.enabled = true;
                //barrel.RegularFire ();
                barrel.sendFireCommand();
			//}
		}

		CmdDestroy();
	}
}
