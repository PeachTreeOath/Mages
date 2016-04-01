using UnityEngine;
using System.Collections;

public class FodderAI : MonoBehaviour {

	public float speed = 1f;
	public float sinAmplitude = 1.0f;
	public float sinFrequency = 1.0f;
	private float horizontalOffset = 0f;
	private float time;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		transform.position -= horizontalOffset * transform.right;
		transform.position += transform.up * speed * Time.deltaTime;
		horizontalOffset = Mathf.Sin (time * sinFrequency * 2 * Mathf.PI) * sinAmplitude;
		transform.position += horizontalOffset * transform.right;
	}
}
