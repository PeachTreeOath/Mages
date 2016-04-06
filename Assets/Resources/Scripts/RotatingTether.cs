using UnityEngine;
using System.Collections;

public class RotatingTether : MonoBehaviour {
    public float angularVelocity = 30;
    public GameObject objectToCircle;

	// Use this for initialization
	void Start () {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.angularVelocity = angularVelocity;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
