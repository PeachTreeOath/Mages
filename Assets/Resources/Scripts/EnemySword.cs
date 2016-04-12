using UnityEngine;
using System.Collections;

public class EnemySword : Enemy
{
	public float moveSpeed;
	public float minimumSpeed;
	public float rotateSpeed;

	private Vector2 prevLocation;
	private Vector2 nextLocation;
	private float targetAngle;
	private float rotationAmount;
	private float targetRotationAmount;
	private Shoot[] barrels;
	private Rigidbody2D body;
	private float timeElapsed;
	private string state = "Dashing";
	private int spinDir;

	// Use this for initialization
	void Start ()
	{
		barrels = GetComponentsInChildren<Shoot> ();
		body = GetComponent<Rigidbody2D> ();
		prevLocation = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		// Stop and spin when slow enough
		if (state == "Dashing" && body.velocity.magnitude < minimumSpeed) {
			foreach (Shoot barrel in barrels) {
				barrel.enabled = false;
			}
			body.velocity = Vector2.zero;
			state = "Spinning";
			ChooseNextLocation ();
		} else if (state == "Spinning") {
			float origAngle = transform.rotation.z;
			float frameRotatedAmount = rotateSpeed * Time.fixedDeltaTime;
			rotationAmount += frameRotatedAmount;
			transform.Rotate (new Vector3 (0, 0, frameRotatedAmount * -spinDir));

			// Resume dashing once target angle reached
			if (rotationAmount >= targetRotationAmount) {
				foreach (Shoot barrel in barrels) {
					barrel.enabled = true;
				}
				transform.rotation = Quaternion.AngleAxis (targetAngle, Vector3.forward);
				Dash ();
			}
		}
	}

	private void Dash ()
	{
		float dist = Vector2.Distance (prevLocation, nextLocation);
		body.AddForce (transform.up * dist * moveSpeed, ForceMode2D.Force);
		state = "Dashing";
	}

	private void ChooseNextLocation ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		GameObject player = players [Random.Range (0, players.Length)];

		if (player != null) {
			Vector3 vectorToTarget = player.transform.position - transform.position;
			spinDir = FindDir (transform.forward, vectorToTarget, transform.up);
			targetAngle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
			rotationAmount = 0;
			targetRotationAmount = Mathf.Abs (Mathf.DeltaAngle (transform.rotation.eulerAngles.z, targetAngle));
			prevLocation = transform.position;
			nextLocation = player.transform.position;
		}
	}

	private int FindDir (Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross (fwd, targetDir);
		float dir = Vector3.Dot (perp, up);

		if (dir > 0) {
			return 1;
		} else if (dir < 0) {
			return -1;
		} else {
			return 0;
		}
	}

}
