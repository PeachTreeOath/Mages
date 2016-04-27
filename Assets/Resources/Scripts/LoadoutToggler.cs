using UnityEngine;
using System.Collections;

public class LoadoutToggler : MonoBehaviour {

	public int value;

	private LoadoutManager mgr;

	// Use this for initialization
	void Start () {
		mgr = GameObject.Find ("LoadoutManager").GetComponent<LoadoutManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer spr in sprites) {
			if (spr.gameObject.name == "iconBorder") {
				spr.enabled = !spr.enabled;
			}
		}

		mgr.Toggle (name);
	}

}
