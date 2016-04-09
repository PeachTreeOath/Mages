using UnityEngine;
using System.Collections;

public class BossGenie : MonoBehaviour {
    public int phase = 0;
    public Transform[] markers;
    public float speed = 2.0F;
    private float journeyLength;
    private int lastIndexReached = 0;
    private float startTime;
    public float pauseTime = 1;
    private bool pauseStarted = false;
    
	// Use this for initialization
	void Start () {
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        int nextIndex = (lastIndexReached + 1) % markers.Length;
        journeyLength = Vector3.Distance(markers[lastIndexReached].position, markers[nextIndex].position);
        float distCovered = (Time.time - startTime) * speed;
        
        if (distCovered / journeyLength >= 1f)
        {
            lastIndexReached = (lastIndexReached + 1) % markers.Length;
            nextIndex = (lastIndexReached + 1) % markers.Length;
            journeyLength = Vector3.Distance(markers[lastIndexReached].position, markers[nextIndex].position);
            distCovered = (Time.time - startTime) * speed;
            startTime = Time.time;
        }

        float fracJourney = distCovered / journeyLength;
        Vector3 origPosition = transform.position;
        transform.position = Vector3.Lerp(markers[lastIndexReached].position, markers[nextIndex].position, fracJourney);

        float debugLength = Vector3.Distance(origPosition, transform.position);
        Debug.Log("DebugLength = " + debugLength);
    }

}
