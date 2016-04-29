using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public GameObject playerPrefab;

	private bool[] playerList = new bool[8];
	private List<Player> playerObjList = new List<Player>();
	private Dictionary<int,HashSet<string>> weaponMap;

	private GameObject normalShot;
	private GameObject twinShot;
	private GameObject spreadShot;
	private GameObject bigShot;
	private GameObject circleNeg;
	private GameObject firestickNeg;
	private GameObject spreadNeg;

	private Sprite normalShotIcon;
	private Sprite twinShotIcon;
	private Sprite spreadShotIcon;
	private Sprite bigShotIcon;
	private Sprite circleNegIcon;
	private Sprite firestickNegIcon;
	private Sprite spreadNegIcon;

	public float timeForWeaponSwitches = 10f;
	private float timeOfLastWeaponSwitch;

	// Use this for initialization
	void Start ()
	{
		if (GlobalObject.instance != null) {
			playerList = GlobalObject.instance.playerList;
			weaponMap = GlobalObject.instance.weaponMap;
		}

		//TODO: THIS IS ONLY FOR TESTING
		playerList [0] = true;
			playerList [1] = true;

		LoadWeaponResources ();
		CreatePlayers ();

		timeOfLastWeaponSwitch = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
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
			AttachWeapons (player);

			if (playerList [4]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 5;
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [1]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 2;
			AttachWeapons (player);

			if (playerList [5]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 6;
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [2]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 3;
			AttachWeapons (player);

			if (playerList [6]) {
				GameObject obj2 = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
				Player player2 = obj.GetComponent<Player> ();
				player2.playerNum = 7;
				AttachWeapons (player2);
			} else {
				player.soloPlay = true;
			}
		}
		if (playerList [3]) {
			GameObject obj = (GameObject)Instantiate (playerPrefab, Vector2.zero, Quaternion.identity);
			Player player = obj.GetComponent<Player> ();
			player.playerNum = 4;
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
		circleNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponCircle");
		firestickNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegFirestick");
		spreadNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegSpread");

		normalShotIcon = Resources.Load<Sprite> ("Textures/shotNormal");
		twinShotIcon = Resources.Load<Sprite> ("Textures/shotTwin");
		spreadShotIcon = Resources.Load<Sprite> ("Textures/shotSpread");
		bigShotIcon = Resources.Load<Sprite> ("Textures/shotBig");
		//circleNegIcon = Resources.Load<Sprite> ("Prefabs/Textures/WeaponSingle");
		//firestickNegIcon = Resources.Load<Sprite> ("Prefabs/Textures/WeaponNegFirestick");
		//spreadNegIcon = Resources.Load<Sprite> ("Prefabs/Textures/WeaponNegSpread");

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
			//player.AddWeapon (((GameObject)Instantiate (bigShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ());
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
				case "eshotMine":
					//newWep = ((GameObject)Instantiate (normalShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
					break;
				case "eshotBurst":
					//newWep = ((GameObject)Instantiate (normalShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
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
}