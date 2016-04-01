using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		OnStartLocalPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		float currSpeed = speed;
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			currSpeed = 1;
		}
		transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal") * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical") * Time.deltaTime * currSpeed);


	}

	public void OnStartLocalPlayer()
	{

		GetComponent<SpriteRenderer> ().material.color = Color.cyan;
	}
		

}
