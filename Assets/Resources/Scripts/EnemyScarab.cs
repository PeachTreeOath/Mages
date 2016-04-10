using UnityEngine;
using System.Collections;

public class EnemyScarab : Enemy {

	public float speed;
	public float sinAmplitude;
	public float sinFrequency ;
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

	public void BeginMoving(float moveSpeed, float amp, float freq)
	{
		speed = moveSpeed;
		sinAmplitude = amp;
		sinFrequency = freq;
	}
		
}
