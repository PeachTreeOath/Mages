using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour
{

	public float speed;
	public float timeToDie;
	public bool unconscious;
	private bool invincible = true;
	private float unconsciousTime;
	private float currTimeToDie;
	private GameManager mgr;
	private Renderer renderer;
	private string[] layers = { "Invisible", "Player" };

	// Use this for initialization
	void Start ()
	{
		mgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		renderer = GetComponent<SpriteRenderer> ().GetComponent<Renderer> ();
		currTimeToDie = timeToDie;
		OnStartLocalPlayer ();
		StartCoroutine (Flash (.2f, 0.05f));
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
			float timeElapsed = Time.time - unconsciousTime;
			if (timeElapsed < timeToDie) {
				transform.localPosition = (Vector2)transform.position + (Random.insideUnitCircle * timeElapsed * 0.02f);
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
		if (invincible) {
			return;
		}

		if (!unconscious) {
			// Go unconscious for a few secs then explode
			unconsciousTime = Time.time;
			unconscious = true;
			GetComponentInChildren<Head> ().enabled = false;
			Shoot[] shooters = GetComponentsInChildren<Shoot> ();
			foreach (Shoot shooter in shooters) {
				shooter.enabled = false;
			}

		}
	}

	private void ActuallyDie ()
	{
		mgr.SpawnPlayer ();
		GetComponent<GibManual> ().Explode ();
	}

	IEnumerator Flash (float time, float intervalTime)
	{
		invincible = true;
		float elapsedTime = 0f;
		int index = 0;
		while (elapsedTime < time) {
			renderer.enabled = !renderer.enabled;

			elapsedTime += Time.deltaTime;
			index++;
			yield return new WaitForSeconds (intervalTime);
		}
		renderer.enabled = true;
		invincible = false;
	}

}
