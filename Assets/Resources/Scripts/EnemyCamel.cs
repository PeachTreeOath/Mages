﻿using UnityEngine;
using System.Collections;

public class EnemyCamel : Enemy {

	private float weaponRotateSpeed;
	private Shoot[] barrels;

	// Use this for initialization
	public void Start () {
		barrels = GetComponentsInChildren<Shoot> ();
	}
	
	// Update is called once per frame
	public void Update () {
		foreach(Shoot barrel in barrels)
		{
			barrel.transform.Rotate (0, 0, weaponRotateSpeed);
		}
	}

	public void BeginMoving(bool isFacingRight, Vector2 moveSpeed, float wepRotateSpeed)
	{
		weaponRotateSpeed = wepRotateSpeed;
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		if (isFacingRight) {
			body.AddForce (moveSpeed.x * Vector2.right);
			GetComponent<SpriteRenderer> ().flipX = true;
		} else {
			body.AddForce (moveSpeed.x * Vector2.left);
		}
		body.AddForce (moveSpeed.y * Vector2.up);
	}
}
