using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossGenie : MonoBehaviour
{
	public float hp;
	public int phase = 0;
	public Transform[] markers;
	public float speed = 2.0F;
	private float journeyLength;
	private int lastIndexReached = 0;
	private float startTime;
	public float pauseTime = 1;
	public GameObject hpCanvas;
	private Image hpMeterImage;
	private bool pauseStarted = false;
	private float totalHp;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
		totalHp = hp;
		hpCanvas.SetActive (true);
		hpMeterImage = hpCanvas.GetComponentInChildren<GameObjectFinder> ().GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		int nextIndex = (lastIndexReached + 1) % markers.Length;
		journeyLength = Vector3.Distance (markers [lastIndexReached].position, markers [nextIndex].position);
		float distCovered = (Time.time - startTime) * speed;
        
		if (distCovered / journeyLength >= 1f) {
			lastIndexReached = (lastIndexReached + 1) % markers.Length;
			nextIndex = (lastIndexReached + 1) % markers.Length;
			journeyLength = Vector3.Distance (markers [lastIndexReached].position, markers [nextIndex].position);
			startTime = Time.time;
			distCovered = (Time.time - startTime) * speed;
            
		}

		float fracJourney = distCovered / journeyLength;
		Vector3 origPosition = transform.position;
		transform.position = Vector3.Lerp (markers [lastIndexReached].position, markers [nextIndex].position, fracJourney);
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		Bullet bullet = col.GetComponent<Bullet> ();

		if (bullet != null) {
			hp -= bullet.damage;
			if (hp <= 0) {
				hpCanvas.SetActive (false);
			} else {
				hpMeterImage.fillAmount = hp / totalHp;
			}
		}
	}
}
