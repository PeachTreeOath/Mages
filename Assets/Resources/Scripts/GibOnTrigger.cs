using UnityEngine;
using System.Collections;

public class GibOnTrigger : MonoBehaviour {

	public GameObject gib;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		if (gib != null) {
			Instantiate (gib, transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}
}
