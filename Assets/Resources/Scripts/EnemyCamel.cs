using UnityEngine;
using System.Collections;

public class EnemyCamel : Enemy {

	public bool isFacingRight;
	public Vector2 moveSpeed;
	public float weaponRotateSpeed;
	private Shoot[] barrels;

	// Use this for initialization
	public override void Start () {
		hp = 1;
		barrels = GetComponentsInChildren<Shoot> ();
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		if (isFacingRight) {
			body.AddForce (moveSpeed.x * Vector2.right);
		} else {
			body.AddForce (moveSpeed.x * Vector2.left);
		}
		body.AddForce (moveSpeed.y * Vector2.up);
	}
	
	// Update is called once per frame
	public override void Update () {
		foreach(Shoot barrel in barrels)
		{
			barrel.transform.Rotate (0, 0, weaponRotateSpeed);
		}
	}
}
