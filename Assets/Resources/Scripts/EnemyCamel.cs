﻿using UnityEngine;
using System.Collections;

public class EnemyCamel : Enemy {

	public bool isFacingRight;
	public float moveSpeed;
	public float weaponRotateSpeed;
	private Shoot[] barrels;

	// Use this for initialization
	public override void Start () {
		hp = 1;
		barrels = GetComponentsInChildren<Shoot> ();
		if (isFacingRight) {
			GetComponent<Rigidbody2D> ().AddForce (moveSpeed * Vector2.right);
		} else {
			GetComponent<Rigidbody2D> ().AddForce (moveSpeed * Vector2.left);
		}

	}
	
	// Update is called once per frame
	public override void Update () {
		foreach(Shoot barrel in barrels)
		{
			//float newRot = barrel.transform.rotation.z + weaponRotateSpeed;
			barrel.transform.Rotate (0, 0, weaponRotateSpeed);
			//barrel.transform.rotation = new Quaternion(barrel.transform.rotation.x, barrel.transform.rotation.y, newRot, barrel.transform.rotation.w);
		}
	}
}
