using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour
{

	public float speed;
	public float timeToDie;
	public bool unconscious;
	private float unconsciousTime;
	private float currTimeToDie;

	// Use this for initialization
	void Start ()
	{
		currTimeToDie = timeToDie;
		OnStartLocalPlayer ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!unconscious) {
			float currSpeed = speed;
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				currSpeed = 1;
			}
			transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal") * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical") * Time.deltaTime * currSpeed);
		} else {
			if (Time.time - unconsciousTime < timeToDie) {
				// health bar that depletes unless revive
			} else {
				ActuallyDie ();
			}
		}
	}

	void LateUpdate ()
	{
		if (transform.position.x < -8.5f) {
			transform.position = new Vector2 (-8.5f, transform.position.y);
		} else if (transform.position.x > 8.5f) {
			transform.position = new Vector2 (8.5f, transform.position.y);
		}

		if (transform.position.y < -4.5f) {
			transform.position = new Vector2 (transform.position.x, -4.5f);
		} else if (transform.position.y > 4.5f) {
			transform.position = new Vector2 (transform.position.x, 4.5f);
		}
	}

	public void OnStartLocalPlayer ()
	{
		GetComponent<SpriteRenderer> ().material.color = Color.cyan;
	}

	public void Die ()
	{
		// Go unconscious for a few secs then explode
		unconsciousTime = Time.time;
		unconscious = true;
		GetComponent<Head> ().enabled = false;
	}

	private void ActuallyDie ()
	{
		
	}
}
