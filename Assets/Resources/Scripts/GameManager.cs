using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public bool easyModeOn = false;
	public GameObject playerPrefab;

	private bool[] playerList = new bool[8];
	public List<Player> playerObjList = new List<Player> ();
	private Dictionary<int,HashSet<string>> weaponMap;

	private GameObject normalShot;
	private GameObject twinShot;
	private GameObject spreadShot;
	private GameObject bigShot;
	private GameObject aegisShot;
	private GameObject mineNegShot;
	private GameObject burstNegShot;
	private GameObject firestickNegShot;
	private GameObject wheelNegShot;

	private Sprite normalShotIcon;
	private Sprite twinShotIcon;
	private Sprite spreadShotIcon;
	private Sprite bigShotIcon;
	private Sprite aegisShotIcon;
	private Sprite mineNegIcon;
	private Sprite burstNegIcon;
	private Sprite firestickNegIcon;
	private Sprite wheelNegIcon;

	public float timeForWeaponSwitches = 10f;
	private float timeOfLastWeaponSwitch;
	private SpriteRenderer retryPanel;
	private int section;
	private MeshRenderer bg;

	public int[] checkPointYValues;

	// Use this for initialization
	void Start ()
	{
		retryPanel = GameObject.Find ("RetryPanel").GetComponent<SpriteRenderer> ();
		bg = GameObject.Find ("BG").GetComponent<MeshRenderer> ();

		if (GlobalObject.instance != null) {
			//TODO
			playerList = GlobalObject.instance.playerList;
			weaponMap = GlobalObject.instance.weaponMap;
			easyModeOn = GlobalObject.instance.easyModeOn;
			section = GlobalObject.instance.section;
		}

		//TODO: THIS IS ONLY FOR TESTING
		//playerList [0] = true;
		//playerList [1] = true;
		//playerList [3] = true;

		SwitchSection (true);
		LoadWeaponResources ();
		CreatePlayers ();
		LoadCheckpoint ();

		timeOfLastWeaponSwitch = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
		if (retryPanel.enabled) {
			for (int i = 0; i < 8; i++) {
				if (i < 4) {
					// Only players 1-4 are allowed to solo controllers
					if (Input.GetButton ("Action_p" + (i + 1) + "_solo")) {
						RestartStage ();
					}
					// Players 1-4 in coop setting use dpad down to action
					else if (Input.GetAxisRaw ("Action_p" + (i + 1)) > 0) {
						RestartStage ();
					}
				} else {
					// Players 5-8 are forced coop setting and use Y to action
					if (Input.GetButton ("Action_p" + (i + 1))) {
						RestartStage ();
					}
				}
			}

			if (Input.GetButtonDown ("Submit")) {
				if (GlobalObject.instance != null) {
					GlobalObject.instance.section = 0;
				}
				SceneManager.LoadScene ("StartMenu");
			}

			return;
		}

		if (Time.time > timeOfLastWeaponSwitch + timeForWeaponSwitches) {
			SwitchWeapons ();
			timeOfLastWeaponSwitch = Time.time;
		}
	}

	// NOTE: This doesn't protect against players 5-8 solo playing, because you're a fool if you decide to only play with half a controller solo
	private void CreatePlayers ()
	{
		if (playerList [0]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 1;
			Material mat = Resources.Load<Material> ("Materials/blueMat");
			player.SetColor (mat);
			AttachWeapons (player);

			if (playerList [4]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 5;
				Material mat2 = Resources.Load<Material> ("Materials/brownMat");
				player2.SetColor (mat2);
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [1]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 2;
			Material mat = Resources.Load<Material> ("Materials/greenMat");
			player.SetColor (mat);
			AttachWeapons (player);

			if (playerList [5]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 6;
				Material mat2 = Resources.Load<Material> ("Materials/pinkMat");
				player2.SetColor (mat2);
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [2]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 3;
			Material mat = Resources.Load<Material> ("Materials/purpleMat");
			player.SetColor (mat);
			AttachWeapons (player);

			if (playerList [6]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 7;
				Material mat2 = Resources.Load<Material> ("Materials/greyMat");
				player2.SetColor (mat2);
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [3]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 4;
			Material mat = Resources.Load<Material> ("Materials/darkBlueMat");
			player.SetColor (mat);
			AttachWeapons (player);

			if (playerList [7]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 8;
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
	}

	private void LoadWeaponResources ()
	{
		normalShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponSingle");
		twinShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponDoubleLaser");
		spreadShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponSpread");
		bigShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponBig");
		aegisShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponAegis");
		mineNegShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegMine");
		burstNegShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegSpread");
		firestickNegShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegFirestick");
		wheelNegShot = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegCircle");

		normalShotIcon = Resources.Load<Sprite> ("Textures/shotNormal");
		twinShotIcon = Resources.Load<Sprite> ("Textures/shotTwin");
		spreadShotIcon = Resources.Load<Sprite> ("Textures/shotSpread");
		bigShotIcon = Resources.Load<Sprite> ("Textures/shotBig");
		aegisShotIcon = Resources.Load<Sprite> ("Textures/shotAegis");
		mineNegIcon = Resources.Load<Sprite> ("Textures/eshotMine");
		burstNegIcon = Resources.Load<Sprite> ("Textures/eshotBurst");
		firestickNegIcon = Resources.Load<Sprite> ("Textures/eshotFirestick");
		wheelNegIcon = Resources.Load<Sprite> ("Textures/eshotWheel");

	}

	public Sprite GetWeaponSprite (string name)
	{
		switch (name) {
		case "WeaponSingle(Clone)":
			return normalShotIcon;
		case "WeaponDoubleLaser(Clone)":
			return twinShotIcon;
		case "WeaponSpread(Clone)":
			return spreadShotIcon;
		case "WeaponBig(Clone)":
			return bigShotIcon;
		case "WeaponAegis(Clone)":
			return aegisShotIcon;
		case "WeaponNegMine(Clone)":
			return mineNegIcon;
		case "WeaponNegSpread(Clone)":
			return burstNegIcon;
		case "WeaponNegFirestick(Clone)":
			return firestickNegIcon;
		case "WeaponNegCircle(Clone)":
			return wheelNegIcon;
		}

		return null;
	}

	private void AttachWeapons (Player player)
	{
		// Debugging, just attach something
		if (weaponMap == null) {
			
			player.AddWeapon (((GameObject)Instantiate (normalShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			player.AddWeapon (((GameObject)Instantiate (twinShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			player.AddWeapon (((GameObject)Instantiate (spreadShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			player.AddWeapon (((GameObject)Instantiate (bigShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			player.AddWeapon (((GameObject)Instantiate (aegisShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
/*
			player.AddWeapon (((GameObject)Instantiate (aegisShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			//player.AddWeapon (((GameObject)Instantiate (mineNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			//player.AddWeapon (((GameObject)Instantiate (burstNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			//player.AddWeapon (((GameObject)Instantiate (firestickNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
			//player.AddWeapon (((GameObject)Instantiate (wheelNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
*/
					} else {
			// When adding a new weapon, map the name of the shot in the Loadout screen to the prefabs that are loaded here
			foreach (string wepName in weaponMap[player.playerNum - 1]) {
				Weapon newWep = null;
				switch (wepName) {
				case "shotNormal":
					newWep = ((GameObject)Instantiate (normalShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "shotTwin":
					newWep = ((GameObject)Instantiate (twinShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "shotSpread":
					newWep = ((GameObject)Instantiate (spreadShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "shotBig":
					newWep = ((GameObject)Instantiate (bigShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "shotAegis":
					newWep = ((GameObject)Instantiate (aegisShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "eshotMine":
					newWep = ((GameObject)Instantiate (mineNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "eshotBurst":
					newWep = ((GameObject)Instantiate (burstNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "eshotFirestick":
					newWep = ((GameObject)Instantiate (firestickNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "eshotWheel":
					newWep = ((GameObject)Instantiate (wheelNegShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				}
			
				if (newWep != null) {
					player.AddWeapon (newWep);
				}
			}
		}
		playerObjList.Add (player);
	}

	private void SwitchWeapons ()
	{
		foreach (Player player in playerObjList) {
			player.SwitchWeapon ();
		}
	}

	public void CheckAllDeaths ()
	{
		bool anyoneAlive = false;
		foreach (Player player in playerObjList) {
			if (player.deathState != DeathState.FINISHED) {
				anyoneAlive = true;
				break;
			}
		}
		if (!anyoneAlive) {
			if (easyModeOn) {
				foreach (Player player in playerObjList) {
					player.Respawn ();
				}
			} else {
				// Show retry modal
				retryPanel.enabled = true;

				// Pause all spawners
				GameObject[] objs = GameObject.FindGameObjectsWithTag ("Spawner");
				foreach (GameObject obj in objs) {
					SpawnObjects spawner = obj.GetComponent<SpawnObjects> ();
					if (spawner != null) {
						spawner.StopMovement ();
					} else {
						BossScroller boss = obj.GetComponent<BossScroller> ();
						if (boss != null) {
							boss.StopMovement ();
						}
					}
				}
			}
		}
	}

	public void GotoSection (int newSection)
	{
		section = newSection;
		if (GlobalObject.instance != null) {
			GlobalObject.instance.section = section;
		}
		SwitchSection (false);
	}

	private void RestartStage ()
	{
		SceneManager.LoadScene ("Game");
	}

	private void LoadCheckpoint ()
	{
		// Shift spawners down then delete anything under a certain point
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		foreach (GameObject spawner in spawners) {
			Vector2 pos = spawner.transform.position;
			pos = new Vector2 (pos.x, pos.y - checkPointYValues [section]);
			if (pos.y < 0) {
				Destroy (spawner);
			} else {
				spawner.transform.position = pos;
			}
		}

	}

	private void SwitchSection (bool fromLoad)
	{
		int newSection = section;
		// If died on a boss and loading, start off in previous section so you can watch transition
		if (fromLoad && (section == 1 || section == 3 || section == 5)) {
			newSection--;
		}
		AudioManager.instance.PlayMusic (newSection);

		//TODO: Possibly an issue with loading bg during section 1 3 5?
		switch (newSection) {
		case 0:	
			break;
		case 1:
			break;
		case 2:
			bg.material.mainTexture = Resources.Load<Texture> ("Textures/BGDesert");
			break;
		case 3:
			break;
		case 4:
			bg.material.mainTexture = Resources.Load<Texture> ("Textures/BGCity");
			break;
		case 5:
			break;
		}	
	}
}