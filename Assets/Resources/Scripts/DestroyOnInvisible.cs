using UnityEngine;
using System.Collections;

public class DestroyOnInvisible : MonoBehaviour {
	public GameObject destroyTarget = null;

	void OnBecameInvisible()
	{
		if (destroyTarget == null) {
			Destroy (gameObject);
		} else {
			Destroy (destroyTarget);
		}
	}
}
