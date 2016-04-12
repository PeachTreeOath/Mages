using UnityEngine;
using System.Collections;

public class AnimationSpeeder : MonoBehaviour {

	public float animationSpeed;

	// Use this for initialization
	void Start () {
		GetComponent<Animator> ().speed = animationSpeed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
