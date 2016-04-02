using UnityEngine;
using System.Collections;

public class GibManual : MonoBehaviour {

	public GameObject gib;

	// Use this for initialization
	void Start () {

	}

	public void Explode () {
	if (gib != null) {
			Instantiate (gib, transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}

}
