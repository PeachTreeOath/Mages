﻿using UnityEngine;
using System.Collections;

public class BossScroller : MonoBehaviour
{
	public float scrollSpeed = 1f;
	public float stopLine = 2.5f;

	private Rigidbody2D rbody;
	private bool paused;
	private GameManager gameMgr;

	// Use this for initialization
	void Start ()
	{
		gameMgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
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
			if (obj.layer == layer1) {
				if (obj.name != "Genie" && obj.name != "Sphinx" && obj.name != "Aladdin") {
					Destroy (obj);
				}
			}
			if (obj.layer == layer2) {
				if (obj.GetComponent<Bullet>() != null && obj.GetComponent<Bullet>().type != 2) {
					Destroy (obj);
				}
			}
		}

		// Destroy spawners too just in case
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		foreach (GameObject spawner in spawners) {
			if (spawner.transform.position.y < 5.25f && spawner.GetInstanceID () != gameObject.GetInstanceID ()) {
				Destroy (spawner);
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
		switch (name) {
		case "BossFightGenie":
			gameMgr.GotoSection (1);
			break;
		case "BossFightSphinx":
			gameMgr.GotoSection (3);
			break;
		case "BossFightAladdin":
			gameMgr.GotoSection (5);
			break;
		}
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
		switch (name) {
		case "BossFightGenie":
			gameMgr.GotoSection (2);
			break;
		case "BossFightSphinx":
			gameMgr.GotoSection (4);
			break;
		case "BossFightAladdin":
			gameMgr.GotoSection (6);
			break;
		}

		Destroy (gameObject);
	}
}
