using UnityEngine;
using System.Collections;

public class LookAtRandomPlayer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		GameObject player = players [Random.Range (0, players.Length)];

		if (player != null) {
			transform.LookAt (player.transform.position);

			Vector3 vectorToTarget = player.transform.position - transform.position;
			float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
			transform.rotation  = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
