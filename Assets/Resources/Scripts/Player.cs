using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : NetLifecycleObj
{

	//[SyncVar]
	public int playerNum;
	public bool soloPlay;
	// 1 person on controller instead of 2
	public PlayerState playerState;
	//[SyncVar]
	public DeathState deathState;
	//not applicable until player is in DYING state
	public bool initDone = false;
	public float speed = 0;
	public float timeToDie = 2.0f;
	public float timeForWeaponSwitches = 10f;
	public List<Weapon> weaponLoadout = new List<Weapon> ();

	private float deathStateTime;
	//used for several timings
	private const float SPAWNING_TIME = 1.0f;
	private const float UNCONSCIOUS_TIME = 2.0f;
	private const float EXPLODE_TIME = 2.0f;
	private Renderer rend;
	private float timeOfLastWeaponSwitch;
	private int currentWeaponIndex = 0;
	private Weapon currentWeapon;

	void Start ()
	{
		//generally this is called when the scene is first loaded
		Debug.Log ("Player Start called");
		SpawnPlayer ();

		Weapon nextWeapon = weaponLoadout [currentWeaponIndex];
		currentWeapon = (Weapon)Instantiate (nextWeapon, transform.position, transform.rotation);
		currentWeapon.transform.parent = this.transform;
		timeOfLastWeaponSwitch = Time.time;
	}

	public void AddWeapons(Weapon weapon)
	{
		weaponLoadout.Add (weapon);
	}

	//Client side code only
	public void SpawnPlayer ()
	{
		Debug.Log ("in spawn player code");
		/*  if (!isLocalPlayer) {
            Debug.LogError("Tried to call spawnPlayer from non-local client");
            return;
        }*/
		playerState = PlayerState.SPAWNING;
		deathState = DeathState.STARTING;
		rend = GetComponent<SpriteRenderer> ().GetComponent<Renderer> ();
		GameObject parent = SpawnDelegate.getInstance ().getPlayerSpawnLocation (playerNum);
		gameObject.transform.SetParent (parent.transform, false);
		initDone = true;
		Debug.Log ("Player init done");
		StartCoroutine (Flash (SPAWNING_TIME, 0.05f));
	}

	public void OnStartLocalPlayer ()
	{
		Debug.Log ("OnStartLocalPlayer called");
		GetComponent<SpriteRenderer> ().material.color = Color.cyan;
	}

	void Update ()
	{
		if (Time.time > timeOfLastWeaponSwitch + timeForWeaponSwitches) {
			//Remove existing weapon
			Debug.Log ("Removing existing weapon.");
			//Weapon currentWeapon = weaponLoadout[currentWeaponIndex];

			if (currentWeapon != null) {
				GameObject go = currentWeapon.gameObject;
				Destroy (go);
			}
			if (currentWeaponIndex == weaponLoadout.Count - 1) {
				currentWeaponIndex = 0;
			} else {
				currentWeaponIndex++;
			}
			Weapon nextWeapon = weaponLoadout [currentWeaponIndex];
			currentWeapon = (Weapon)Instantiate (nextWeapon, transform.position, transform.rotation);
			currentWeapon.transform.parent = this.transform;

			timeOfLastWeaponSwitch = Time.time;
		}
		if (!initDone) {
			return;
		}
		/*
        //Only client code from here
        if (!isLocalPlayer || !isClient) {
            return;
        }
		*/
		//Debug.Log("Player state is " + playerState);

		switch (playerState) {
		case PlayerState.SPAWNING:
		case PlayerState.NEUTRAL:
		case PlayerState.INVINCIBLE:
			float currSpeed = speed;
			// Only players 1-4 are allowed to solo controllers
			if (soloPlay) {
				if (Input.GetButton ("Action_p" + playerNum + "_solo")) {
					currSpeed = speed * 0.33f;
				}
				transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal_p" + playerNum + "_solo") * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical_p" + playerNum + "_solo") * Time.deltaTime * currSpeed);
			} else {
				if (playerNum < 4) {
					// Players 1-4 in coop setting use dpad down to action
					if (Input.GetAxisRaw ("Action_p" + playerNum) > 0) {
						currSpeed = speed * 0.33f;
					}
				} else {
					// Players 5-8 are forced coop setting and use Y to action
					if (Input.GetButton ("Action_p" + playerNum)) {
						currSpeed = speed * 0.33f;
					}
				}
				transform.position = (Vector2)(transform.position) + new Vector2 (Input.GetAxis ("Horizontal_p" + playerNum) * Time.deltaTime * currSpeed, Input.GetAxis ("Vertical_p" + playerNum) * Time.deltaTime * currSpeed);
			}
			break;
	
		case PlayerState.DYING:
			updateDeathState ();
			break;

		default:
			Debug.Log ("Unhandled playerState " + playerState);
			break;
		}
	}


	void LateUpdate ()
	{
		if (!initDone) {
			return;
		}
		/*
        if (!isLocalPlayer) {
            return;
        }
*/
		// if (isClient) {
		if (transform.position.x < -8.5f) {
			transform.position = new Vector2 (-8.5f, transform.position.y);
		} else if (transform.position.x > 8.5f) {
			transform.position = new Vector2 (8.5f, transform.position.y);
		}

		if (transform.position.y < -4.5f) {
			transform.position = new Vector2 (transform.position.x, -4.5f);
		} else if (transform.position.y > 4.5f) {
			transform.position = new Vector2 (transform.position.x, 4.5f);
		}
		//  }
	}

	public override void endLife ()
	{
		Debug.Log ("Player received request to die");
		Die ();
	}

	public override void createLife ()
	{
		Debug.Log ("Player received request to respawn");
		CmdServerRespawn ();
	}

	//  [Command] //enter server mode
	private void CmdServerRespawn ()
	{
		serverRespawn ();
	}

	//This is called on collision trigger by the head
	public void Die ()
	{
		// if (isClient) {
		//Can only die from the neutral state currently
		if (playerState == PlayerState.NEUTRAL) {
			Debug.Log ("Carrying on with Killing player.");
			playerState = PlayerState.DYING;
			CmdDie (); //offload to server so everyone can see death animation
		} else {
			Debug.Log ("Don't need to die, we aren't in neutral state.");
		}
		// }
	}

	// [Command] //run on server (called from client, obviously)
	private void CmdDie ()
	{
		//time transitions between states are handled in update
		if (deathState == DeathState.STARTING) {
			Debug.Log ("CmdDie starting");
			// Go unconscious for a few secs then explode
			deathStateTime = Time.time;
			deathState = DeathState.UNCONSCIOUS;
			GetComponentInChildren<Head> ().enabled = false;
			Shoot[] shooters = GetComponentsInChildren<Shoot> ();
			foreach (Shoot shooter in shooters) {
				shooter.enabled = false;
			}
		} else {
			Debug.LogError ("CmdDie called while death was already happening.  Not an error, but get your shit together");
		}
	}

	//This code is executed on the server only
	private void updateDeathState ()
	{
		/*  if (!isServer) {
            Debug.LogError("Shits broke, this should only be called in a server context");
        }*/
		//Debug.Log("Updating death state");
		//this needs work
		float timeElapsed = Time.time - deathStateTime;
		if (timeElapsed < timeToDie) {
			transform.position = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle * timeElapsed * 0.02f);
		} else {
			GetComponent<GibManual> ().Explode ();
			deathState = DeathState.FINISHED;
			serverRespawn ();
		}
	}

	private void serverRespawn ()
	{
		//still on the server
		Debug.Log ("Server respawning player");
		RpcRespawn ();
	}

	//[ClientRpc]
	private void RpcRespawn ()
	{
		//if (isLocalPlayer && deathState == DeathState.FINISHED) {
		if (deathState == DeathState.FINISHED) {
			deathState = DeathState.STARTING;
			initDone = false;
			SpawnPlayer ();
		}
	}

	IEnumerator Flash (float time, float intervalTime)
	{
		float doneTime = Time.time + time;
		while (Time.time < doneTime) {
			rend.enabled = !rend.enabled;
			yield return new WaitForSeconds (intervalTime);
		}
		rend.enabled = true;
		playerState = PlayerState.NEUTRAL;
	}

}
