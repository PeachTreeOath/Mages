using UnityEngine;
using System.Collections;

public class BossScroller : MonoBehaviour
{
	public float scrollSpeed = 1f;
	public float stopLine = 2.5f;

	private Rigidbody2D rbody;
	private bool paused;

	// Use this for initialization
	void Start ()
	{
		rbody = GetComponent<Rigidbody2D> ();
		ResumeMovement ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Pause all spawners
		if (!paused && transform.position.y < stopLine) {
			paused = true;
			transform.position = new Vector2 (0, stopLine);
			GameObject[] objs = GameObject.FindGameObjectsWithTag ("Spawner");
			foreach (GameObject obj in objs) {
				SpawnObjects spawner = obj.GetComponent<SpawnObjects> ();
				if (spawner != null) {
					spawner.StopMovement ();
				} else {
					BossScroller boss = obj.GetComponent<BossScroller> ();
					if (boss != null) {
						boss.StopMovement ();
					}
				}
			}
			DestroyAllEnemies ();
			BossActivate ();
		}

		//TODO: Remove this
		if (paused && Input.GetKeyDown (KeyCode.G)) {
			BossDeactivate ();
		}
	}

	public void DestroyAllEnemies ()
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		int layer1 = LayerMask.NameToLayer ("Enemies");
		int layer2 = LayerMask.NameToLayer ("EnemyBullets");
		foreach (GameObject obj in objs) {
			if (obj.layer == layer1 || obj.layer == layer2) {
				if (obj.name != "Genie" && obj.name != "Sphinx" && obj.name != "Aladdin") {
					Destroy (obj);
				}
			}
		}
	}

	public void StopMovement ()
	{
		rbody.velocity = Vector2.zero;
	}

	public void ResumeMovement ()
	{
		rbody.velocity = new Vector2 (0, -1) * scrollSpeed; //This is really down
	}

	private void BossActivate ()
	{
		GetComponentInChildren<Boss> ().enabled = true;
		GetComponentInChildren<Collider2D> ().enabled = true;
	}

	// Called from Boss script when hp < 0
	public void BossDeactivate ()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Spawner");
		foreach (GameObject obj in objs) {
			SpawnObjects spawner = obj.GetComponent<SpawnObjects> ();
			if (spawner != null) {
				if (spawner.transform.position.y < 6) {
					Destroy (spawner.gameObject);
					continue;
				}
				spawner.ResumeMovement ();
			} else {
				BossScroller boss = obj.GetComponent<BossScroller> ();
				if (boss != null) {
					boss.ResumeMovement ();
				}
			}
		}

		Destroy (transform.parent.gameObject);
	}
}
