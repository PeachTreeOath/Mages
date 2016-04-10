using UnityEngine;
using System.Collections;

public class EnemySnake : Enemy
{
	public Vector2 topLeftBound;
	public Vector2 bottomRightBound;
	public float moveSpeed;
	public float moveDistance;
	public float moveDelay;

	private Vector2 prevLocation;
	private Vector2 nextLocation;
	private bool needLocation = true;
	private float timeElapsed;
	private float distance;

	// Use this for initialization
	void Start ()
	{
		prevLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (needLocation) {
			ChooseNextLocation ();
		}

		timeElapsed += Time.deltaTime * moveSpeed;

		transform.position = Vector2.Lerp (prevLocation, nextLocation, timeElapsed);
		if (timeElapsed > 1 + moveDelay) {
			prevLocation = nextLocation;
			needLocation = true;
		}
	}

	private void ChooseNextLocation ()
	{
		Vector2 newLoc;
		// yolo levels of risky
		while (true) {
			newLoc = (UnityEngine.Random.insideUnitCircle * moveDistance) + (Vector2)transform.position;
			if (newLoc.x > topLeftBound.x && newLoc.x < bottomRightBound.x &&
			    newLoc.y < topLeftBound.y && newLoc.y > bottomRightBound.y) {
				break;
			}
		}
		nextLocation = newLoc;
		needLocation = false;
		timeElapsed = 0;
	}
}
