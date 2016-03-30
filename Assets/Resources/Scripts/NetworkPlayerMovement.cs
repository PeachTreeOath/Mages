using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayerMovement : NetworkBehaviour {

	public float speed;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal") * Time.deltaTime * speed, Input.GetAxis ("Vertical") * Time.deltaTime * speed);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer ();

		GetComponent<SpriteRenderer> ().material.color = Color.blue;
	}

	[Command]
	private void CmdFire()
	{
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, Quaternion.identity);

		bullet.GetComponent<Rigidbody2D> ().velocity = bullet.transform.forward * 6.0f;

		NetworkServer.Spawn (bullet);

		Destroy (bullet, 2);
	}
}
