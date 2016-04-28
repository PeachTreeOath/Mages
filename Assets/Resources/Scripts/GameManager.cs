using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public GameObject playerPrefab;

	private bool[] playerList = new bool[8];
	private Dictionary<int,HashSet<string>> weaponMap;

	private GameObject normalShot;
	private GameObject twinShot;
	private GameObject spreadShot;
	private GameObject bigShot;
	private GameObject circleNeg;
	private GameObject firestickNeg;
	private GameObject spreadNeg;

	// Use this for initialization
	void Start ()
	{
		if (GlobalObject.instance != null) {
			playerList = GlobalObject.instance.playerList;
			weaponMap = GlobalObject.instance.weaponMap;
		}

		//TODO: THIS IS ONLY FOR TESTING
		playerList [0] = true;
	//	playerList [1] = true;

		LoadWeaponResources ();
		CreatePlayers ();
	}

	// Update is called once per frame
	void Update ()
	{

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
		circleNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponSingle");
		firestickNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegFirestick");
		spreadNeg = Resources.Load<GameObject> ("Prefabs/Weapons/WeaponNegSpread");
	}

	private void AttachWeapons (Player player)
	{
		// Debugging, just attach something
		if (weaponMap == null) {
			Weapon newWep = ((GameObject)Instantiate (normalShot, Vector2.zero, Quaternion.identity)).GetComponent<Weapon> ();
			player.AddWeapon (newWep);
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
	}
}