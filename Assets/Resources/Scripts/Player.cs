using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	// Use this for initialization
	void Start () {
		OnStartLocalPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		float currSpeed = speed;
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			currSpeed = 1;
		}
		transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal") * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical") * Time.deltaTime * currSpeed);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}
	}

	public void OnStartLocalPlayer()
	{

		//GetComponent<SpriteRenderer> ().material.color = Color.blue;
	}
		
	private void CmdFire()
	{
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, Quaternion.identity);
		bullet.GetComponent<Bullet> ().SetType (0);
		bullet.GetComponent<Bullet> ().Fire ();
		Destroy (bullet, 2);
	}
}
