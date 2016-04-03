using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameNetMan : NetworkManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Debug.Log("ServerAddPlayer called");
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
