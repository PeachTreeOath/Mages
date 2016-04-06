using UnityEngine;
using System.Collections;

public class EnemySnake : MonoBehaviour
{

	public Vector2 topLeftBound;
	public Vector2 bottomRightBound;
	public float moveSpeed;
	public float moveDistance;
	public float moveDelay;

	private Vector2 nextLocation;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (nextLocation == null) {

		}
	}

	private void ChooseNextLocation ()
	{
		Vector2 newLoc;
		while (true) {
			newLoc = (UnityEngine.Random.insideUnitCircle * moveDistance) + (Vector2)transform.position;
			if (newLoc.x > topLeftBound.x && newLoc.x < bottomRightBound.x &&
			    newLoc.y < topLeftBound.y && newLoc.y > bottomRightBound.y) {
				break;
			}
		}


	}
}
