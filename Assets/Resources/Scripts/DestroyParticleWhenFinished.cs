﻿using UnityEngine;
using System.Collections;

public class DestroyParticleWhenFinished : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<ParticleSystem> ().isPlaying) {
			Destroy (gameObject);
		}
	}
}
