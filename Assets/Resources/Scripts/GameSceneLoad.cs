using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

//Attach this to an object in the scene that is instantiated once when the scene is loaded
public class GameSceneLoad : NetworkBehaviour {

    void Awake() {
        Debug.Log("Current scene is " + SceneManager.GetActiveScene().name);
    }

    void Start() {
        if (isServer) {
            Debug.Log("Scene load startup, spawning players");
            NetworkServer.Spawn(NetworkManager.singleton.playerPrefab);
        }
        if(isClient) {
            Debug.Log("Adding client player");
            ClientScene.AddPlayer(ClientScene.readyConnection, 0);
        }
    }

    void Update() {
    }
}
