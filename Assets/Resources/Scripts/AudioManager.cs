﻿using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	
	public static AudioManager instance;

	private AudioSource audioSrc;
	private AudioClip theme1;
	private AudioClip theme2;
	private AudioClip theme3;
	private AudioClip theme4;
	private AudioClip theme5;
	private AudioClip theme6;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start ()
	{	
		audioSrc = GetComponent<AudioSource> ();
		theme1 = Resources.Load<AudioClip> ("Sounds/Cave_of_Wonders");
		theme2 = Resources.Load<AudioClip> ("Sounds/Genie_Nightmare");
		theme3 = Resources.Load<AudioClip> ("Sounds/Desert_Heat");
		theme4 = Resources.Load<AudioClip> ("Sounds/Riddle_of_the_Sphinx");
		theme5 = Resources.Load<AudioClip> ("Sounds/Bizarre_Bazaar");
		theme6 = Resources.Load<AudioClip> ("Sounds/Dark_Aladdin");
	}

	public void PlayMusic (int theme)
	{
		switch (theme) {
		case 0:
			audioSrc.clip = theme1;
			break;
		case 1:
			audioSrc.clip = theme2;
			break;
		case 2:
			audioSrc.clip = theme3;
			break;
		case 3:
			audioSrc.clip = theme4;
			break;
		case 4:
			audioSrc.clip = theme5;
			break;
		case 5:
			audioSrc.clip = theme6;
			break;
		}
		audioSrc.Play ();
	}
}
