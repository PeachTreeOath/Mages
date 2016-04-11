using UnityEngine;
using System.Collections;

public class GarbageCollector : MonoBehaviour {

	public Vector2 topLeftBounds;
	public Vector2 bottomRightBounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("DeletePastBoundary");

		foreach(GameObject obj in objs)
		{
			if (obj.transform.position.x < topLeftBounds.x || obj.transform.position.x > bottomRightBounds.x ||
			   obj.transform.position.y > topLeftBounds.y || obj.transform.position.y < bottomRightBounds.y) {
				Destroy (obj);
			}
		}
	}
}
