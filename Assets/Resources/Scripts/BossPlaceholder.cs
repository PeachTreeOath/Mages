using UnityEngine;
using System.Collections;

public class BossPlaceholder : MonoBehaviour
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
			GameObject[] objs = GameObject.FindGameObjectsWithTag ("Spawner");
			foreach (GameObject obj in objs) {
				SpawnObjects spawner = obj.GetComponent<SpawnObjects> ();
				if (spawner != null) {
					spawner.StopMovement ();
				} else {
					BossPlaceholder boss = obj.GetComponent<BossPlaceholder> ();
					if (boss != null) {
						boss.StopMovement ();
					}
				}
			}
			DestroyAllEnemies ();
		}

		if (paused && Input.GetKeyDown (KeyCode.G)) {
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
					BossPlaceholder boss = obj.GetComponent<BossPlaceholder> ();
					if (boss != null) {
						boss.ResumeMovement ();
					}
				}
			}

			Destroy (this);
		}
	}

	public void DestroyAllEnemies()
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		int layer1 = LayerMask.NameToLayer ("Enemies");
		int layer2 = LayerMask.NameToLayer ("EnemyBullets");
		foreach(GameObject obj in objs)
		{
			if (obj.layer == layer1 || obj.layer == layer2) {
				Destroy (obj);
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
}
