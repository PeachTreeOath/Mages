using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : NetLifecycleObj
{

	public int playerNum;
	public bool soloPlay;
	// 1 person on controller instead of 2
	public PlayerState playerState;
	public DeathState deathState;
	//not applicable until player is in DYING state
	public bool initDone = false;
	public float speed = 0;
	public float timeToDie = 2.0f;
	public List<Weapon> weaponLoadout = new List<Weapon> ();
	public float reviveDistance = 1f;

	private float deathStateTime;
	private float reviveStateTime;

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

	void Start ()
	{
		gameMgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		nextWeaponIcon = transform.Find ("WeaponIcon").GetComponent<SpriteRenderer> ();
		SpawnPlayer ();

		int rand = UnityEngine.Random.Range (0, weaponLoadout.Count);
		nextWeapon = weaponLoadout [rand];
		ToggleBarrels (nextWeapon, true);
		currentWeapon = nextWeapon;
	}

	void Update ()
	{
		if (!initDone) {
			return;
		}

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
		case PlayerState.REVIVING:
			float reviveDelta = (Time.time - reviveStateTime) * 1f;
			transform.position = Vector2.Lerp (reviveActivatePosition, reviveSlot.transform.position, reviveDelta);
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
		Shoot[] barrels = weapon.GetComponentsInChildren<Shoot> ();
		foreach (Shoot barrel in barrels) {
			barrel.enabled = enable;
		}
	}

	public void SpawnPlayer ()
	{
		playerState = PlayerState.SPAWNING;
		deathState = DeathState.STARTING;
		rend = GetComponent<SpriteRenderer> ().GetComponent<Renderer> ();
		GameObject parent = SpawnDelegate.getInstance ().getPlayerSpawnLocation (playerNum);
		gameObject.transform.SetParent (parent.transform, false);
		initDone = true;
		StartCoroutine (Flash (SPAWNING_TIME, 0.05f));
	}

	// Only previews next weapon icon
	public void SwitchWeapon ()
	{
		int rand = UnityEngine.Random.Range (0, weaponLoadout.Count);
		nextWeapon = weaponLoadout [rand];
		Sprite nextSpr = gameMgr.GetWeaponSprite (nextWeapon.name);
		nextWeaponIcon.sprite = nextSpr;
		nextWeaponIcon.enabled = true;
		Invoke ("StartWeaponFlash", WEAPON_SWITCH_DELAY);
	}

	private void StartWeaponFlash ()
	{
		StartCoroutine (FlashIcon (WEAPON_SWITCH_TIME, 0.05f));
	}

	IEnumerator FlashIcon (float time, float intervalTime)
	{
		float doneTime = Time.time + time;
		while (Time.time < doneTime) {
			nextWeaponIcon.enabled = !nextWeaponIcon.enabled;
			yield return new WaitForSeconds (intervalTime);
		}
		nextWeaponIcon.enabled = false;
		CompleteSwitchWeapon ();
	}

	// Actually swaps weapons
	private void CompleteSwitchWeapon ()
	{
		ToggleBarrels (currentWeapon, false);
		ToggleBarrels (nextWeapon, true);
		currentWeapon = nextWeapon;
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

	private void CmdServerRespawn ()
	{
		Respawn ();
	}

	//This is called on collision trigger by the head
	public void Die ()
	{
		//Can only die from the neutral state currently
		if (playerState == PlayerState.NEUTRAL) {
			Debug.Log ("Carrying on with Killing player.");
			playerState = PlayerState.DYING;
			CmdDie (); //offload to server so everyone can see death animation
		} else {
			Debug.Log ("Don't need to die, we aren't in neutral state.");
		}
	}

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
		bool actionPressed = false;
		if (soloPlay) {
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
				reviveStateTime = Time.time;
				playerState = PlayerState.REVIVING;
				playerRevivingMe = closestPlayer;
				reviveSlot = playerRevivingMe.GetComponent<Player> ().GetFreeReviveSlot (gameObject);
				reviveActivatePosition = transform.position;
				rend.enabled = false;
				return;
			}
		}

		float timeElapsed = Time.time - deathStateTime;
		if (timeElapsed < timeToDie) {
			transform.position = (Vector2)transform.position + (UnityEngine.Random.insideUnitCircle * timeElapsed * 0.02f);
		} else {
			GetComponent<GibManual> ().Explode ();
			deathState = DeathState.FINISHED;
			Respawn ();
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
			float dist = Vector2.Distance (transform.position, obj.transform.position);
			if (dist < reviveDistance) {
				if (closestObj == null || dist < minDist) {
					closestObj = obj;
					minDist = dist;
				}
			}
		}

		return closestObj;
	}

	// Give a random revive slot to a player trying to revive
	public ReviveSlot GetFreeReviveSlot (GameObject callingPlayer)
	{
		Transform slots = transform.Find ("ReviveSlots");
		ReviveSlot[] reviveSlots = slots.gameObject.GetComponentsInChildren<ReviveSlot> ();
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

	private void Respawn ()
	{
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
