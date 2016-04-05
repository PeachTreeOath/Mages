using UnityEngine;
using System.Collections;

public class EnemyCamel : Enemy {

	private float weaponRotateSpeed;
	private Shoot[] barrels;

	// Use this for initialization
	public override void Start () {
		hp = 1;
		barrels = GetComponentsInChildren<Shoot> ();
	}
	
	// Update is called once per frame
	public override void Update () {
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
			foreach(Shoot barrel in barrels)
			{
				barrel.transform.position = new Vector2 (-barrel.transform.position.x, -barrel.transform.position.y);
			}
		} else {
			body.AddForce (moveSpeed.x * Vector2.left);
		}
		body.AddForce (moveSpeed.y * Vector2.up);
	}
}
