﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : NetLifecycleObj
{

	public int playerNum;
	// 1 person on controller instead of 2
	public bool soloPlay;
	public PlayerState playerState;
	//not applicable until player is in DYING state
	public DeathState deathState;
	public bool initDone = false;
	public float speed = 0;
	public float timeToDie = 3.0f;
	public List<Weapon> weaponLoadout = new List<Weapon> ();
	public float reviveDistance = 1f;
	public float timeToRevive = 5f;
	public float addlTimeToRevive = 3f;

	// Revive time changes as player hops from carpet to carpet
	private float currTimeToRevive;
	private float reviveTimeElapsed;
	private float travelTimeElapsed;
	private float deathStateTime;

	//used for several timings
	private const float SPAWNING_TIME = 1.0f;
	private const float UNCONSCIOUS_TIME = 2.0f;
	private const float EXPLODE_TIME = 2.0f;
	private const float WEAPON_SWITCH_DELAY = 2.0f;
	private const float WEAPON_SWITCH_TIME = 1.0f;
	private Renderer rend;
	private Weapon currentWeapon;
	private Weapon nextWeapon;
	private GameManager gameMgr;
	private SpriteRenderer nextWeaponIcon;
	private GameObject playerRevivingMe;
	private ReviveSlot reviveSlot;
	private Vector2 reviveActivatePosition;
	private Head head;
	private ReviveSlot[] reviveSlots;

	void Start ()
	{
		gameMgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		nextWeaponIcon = transform.Find ("WeaponIcon").GetComponent<SpriteRenderer> ();
		Transform slots = transform.Find ("ReviveSlots");
		reviveSlots = slots.gameObject.GetComponentsInChildren<ReviveSlot> ();
		if (head == null) {
			head = GetComponentInChildren<Head> ();
		}
		int rand = UnityEngine.Random.Range (0, weaponLoadout.Count);
		nextWeapon = weaponLoadout [rand];

		currentWeapon = nextWeapon;

		SpawnPlayer ();
	
	}

	void Update ()
	{
		if (!initDone) {
			return;
		}

		switch (playerState) {
		case PlayerState.SPAWNING:
		case PlayerState.NEUTRAL:
			float currSpeed = speed;
			// Only players 1-4 are allowed to solo controllers
			if (soloPlay && playerNum < 4) {
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
		case PlayerState.REVIVING:
			// Handled in lateupdate since the reviver player might be moving
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

		if (playerState == PlayerState.REVIVING) {
			reviveTimeElapsed += Time.deltaTime; // Didn't multiply by time scaling cuz this looks good as is
			travelTimeElapsed += Time.deltaTime;
			transform.position = Vector2.Lerp (reviveActivatePosition, reviveSlot.transform.position, travelTimeElapsed);
			if (reviveTimeElapsed >= currTimeToRevive) {
				SpawnPlayer ();
			}
		}

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
	}

	public void AddWeapon (Weapon weapon)
	{
		ToggleBarrels (weapon, false);
		weapon.transform.SetParent (this.transform);
		weaponLoadout.Add (weapon);
	}

	private void ToggleBarrels (Weapon weapon, bool enable)
	{
		Barrel[] barrels = weapon.GetComponentsInChildren<Barrel> ();
		foreach (Barrel barrel in barrels) {
			barrel.enabled = enable;
		}
	}
	private void DestroyBarrels ()
	{
		Barrel[] barrels = this.GetComponentsInChildren<Barrel> ();
		foreach (Barrel barrel in barrels) {
			Destroy (barrel.gameObject);
		}
	}
	public void SpawnPlayer ()
	{
		currTimeToRevive = 0;
		reviveTimeElapsed = 0;
		travelTimeElapsed = 0;
		if (reviveSlot != null) {
			reviveSlot.RemovePlayer ();
			reviveSlot = null;
		} else {
			GameObject parent = SpawnDelegate.getInstance ().getPlayerSpawnLocation (playerNum);
			gameObject.transform.SetParent (parent.transform, false);
			gameObject.transform.position = parent.transform.position;
		}
		playerRevivingMe = null;
		playerState = PlayerState.SPAWNING;
		deathState = DeathState.STARTING;
		if (rend == null) {
			rend = GetComponent<SpriteRenderer> ().GetComponent<Renderer> ();
		}
		ToggleBarrels (currentWeapon, true);
		rend.enabled = true;
		head.GetComponent<SpriteRenderer> ().enabled = true;
		initDone = true;
		StartCoroutine (Flash (SPAWNING_TIME, 0.05f));
	}

	// Only previews next weapon icon
	public void SwitchWeapon ()
	{
		if (playerState == PlayerState.NEUTRAL || playerState == PlayerState.SPAWNING) {
			int rand = UnityEngine.Random.Range (0, weaponLoadout.Count);
			nextWeapon = weaponLoadout [rand];
			Sprite nextSpr = gameMgr.GetWeaponSprite (nextWeapon.name);
			nextWeaponIcon.sprite = nextSpr;
			nextWeaponIcon.enabled = true;
			Invoke ("StartWeaponFlash", WEAPON_SWITCH_DELAY);
		}
	}

	private void StartWeaponFlash ()
	{
		StartCoroutine (FlashIcon (WEAPON_SWITCH_TIME, 0.05f));
	}

	IEnumerator FlashIcon (float time, float intervalTime)
	{
		float doneTime = Time.time + time;
		while (Time.time < doneTime) {
			if (playerState == PlayerState.NEUTRAL || playerState == PlayerState.SPAWNING) {
				nextWeaponIcon.enabled = !nextWeaponIcon.enabled;
			}
			yield return new WaitForSeconds (intervalTime);
		}
		nextWeaponIcon.enabled = false;
		CompleteSwitchWeapon ();
	}

	// Actually swaps weapons
	private void CompleteSwitchWeapon ()
	{
		if (playerState == PlayerState.NEUTRAL || playerState == PlayerState.SPAWNING) {
			ToggleBarrels (currentWeapon, false);
			ToggleBarrels (nextWeapon, true);
			currentWeapon = nextWeapon;
		}
	}

	public override void endLife ()
	{
		Debug.Log ("Player received request to die");
		Die ();
	}

	public override void createLife ()
	{
		Debug.Log ("Player received request to respawn");
		Respawn ();
	}

	//This is called on collision trigger by the head
	public void Die ()
	{
		if (playerState == PlayerState.NEUTRAL) {
			Debug.Log ("Carrying on with Killing player.");
			playerState = PlayerState.DYING;
			CmdDie (); //offload to server so everyone can see death animation
		} else {
			Debug.Log ("Don't need to die, we aren't in neutral state.");
		}
	}

	//Death from non-bullets aka parent death
	public void ForceDie ()
	{
		playerState = PlayerState.DYING;
		deathState = DeathState.STARTING;
		CmdDie ();
	}

	private void CmdDie ()
	{
		//time transitions between states are handled in update
		DestroyBarrels();
		if (deathState == DeathState.STARTING) {
			Debug.Log ("CmdDie starting");
			// Go unconscious for a few secs then explode
			deathStateTime = Time.time;
			deathState = DeathState.UNCONSCIOUS;
			head.GetComponent<CircleCollider2D> ().enabled = false;
			nextWeaponIcon.enabled = false;

			foreach (ReviveSlot slot in reviveSlots) {
				slot.KillPlayer ();
			}

			Shoot[] shooters = GetComponentsInChildren<Shoot> ();
			foreach (Shoot shooter in shooters) {
				shooter.enabled = false;
			}
		} else {
			Debug.LogError ("CmdDie called while death was already happening.  Not an error, but get your shit together");
		}
	}

	private void updateDeathState ()
	{
		if (deathState == DeathState.FINISHED) {
			return;
		}

		bool actionPressed = false;
		if (soloPlay &&  playerNum < 4)  {
			if (Input.GetButton ("Action_p" + playerNum + "_solo")) {
				actionPressed = true;
			}
		} else {
			if (playerNum < 4) {
				// Players 1-4 in coop setting use dpad down to action
				if (Input.GetAxisRaw ("Action_p" + playerNum) > 0) {
					actionPressed = true;
				}
			} else {
				// Players 5-8 are forced coop setting and use Y to action
				if (Input.GetButton ("Action_p" + playerNum)) {
					actionPressed = true;
				}
			}
		}

		if (actionPressed) {
			GameObject closestPlayer = FindClosestPlayer ();
			if (closestPlayer != null) {
				if (currTimeToRevive == 0) {
					currTimeToRevive = timeToRevive;
				} else {
					currTimeToRevive += addlTimeToRevive;
				}

				playerState = PlayerState.REVIVING;
				playerRevivingMe = closestPlayer;
				if (reviveSlot != null) {
					reviveSlot.RemovePlayer ();
				}
				reviveSlot = playerRevivingMe.GetComponent<Player> ().GetFreeReviveSlot (gameObject);
				reviveActivatePosition = transform.position;
				rend.enabled = false;
				travelTimeElapsed = 0;
				return;
			}
		}

		float timeElapsed = Time.time - deathStateTime;
		if (timeElapsed < timeToDie) {
			transform.position = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle * timeElapsed * 0.02f);
		} else {
			GetComponent<GibManual> ().Explode ();
			deathState = DeathState.FINISHED;
			rend.enabled = false;
			head.GetComponent<SpriteRenderer> ().enabled = false;
			gameMgr.CheckAllDeaths ();
		}
	}

	private GameObject FindClosestPlayer ()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Player");
		GameObject closestObj = null;
		float minDist = 100;
		foreach (GameObject obj in objs) {
			if (obj.GetInstanceID () == gameObject.GetInstanceID ()) {
				continue;
			}
			PlayerState state = obj.GetComponent<Player> ().playerState;
			if (state == PlayerState.NEUTRAL || state == PlayerState.SPAWNING) {
				float dist = Vector2.Distance (transform.position, obj.transform.position);
				if (dist < reviveDistance) {
					if (closestObj == null || dist < minDist) {
						closestObj = obj;
						minDist = dist;
					}
				}
			}
		}
		return closestObj;
	}

	// Give a random revive slot to a player trying to revive
	public ReviveSlot GetFreeReviveSlot (GameObject callingPlayer)
	{
		int slot = UnityEngine.Random.Range (0, reviveSlots.Length);
		GameObject player = reviveSlots [slot].player;
		while (player != null) {
			slot = UnityEngine.Random.Range (0, reviveSlots.Length);
			player = reviveSlots [slot].player;
		}
		// player has to be null at this point
		reviveSlots [slot].player = callingPlayer;
		return reviveSlots [slot];
	}

	public void Respawn ()
	{
		if (deathState == DeathState.FINISHED || playerState == PlayerState.REVIVING) {
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
		head.GetComponent<CircleCollider2D> ().enabled = true;
	}

	public void SetColor (Material mat)
	{
		GetComponent<SpriteRenderer> ().material = mat;
		if (head == null) {
			head = GetComponentInChildren<Head> ();
		}
		head.GetComponent<SpriteRenderer> ().material = mat;
	}
}
