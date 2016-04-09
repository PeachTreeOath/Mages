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
<<<<<<< HEAD
        Debug.Log("GameSceneLoad Start called");
        doInitAtSomePoint();
    }

    void doInitAtSomePoint() {
        Debug.Log("Scene load startup");
        //yield return new WaitForSeconds(2);  //wait for shit
=======
>>>>>>> master
        if (isServer) {
            Debug.Log("Scene load startup, spawning players");
            NetworkServer.Spawn(NetworkManager.singleton.playerPrefab);
        }
        if(isClient) {
<<<<<<< HEAD
            if(NetworkManager.singleton.numPlayers <= 0) {
                Debug.Log("Adding client player");
                ClientScene.AddPlayer(ClientScene.readyConnection, 0); //Players must be added before being ready
            }
            if(!ClientScene.ready) {
                Debug.Log("Player added, setting player to ready");
                ClientScene.Ready(ClientScene.readyConnection);  //It is an error to call this on an already "ready" connection
            }
            Debug.Log("Player set to ready");
=======
            Debug.Log("Adding client player");
            ClientScene.AddPlayer(ClientScene.readyConnection, 0);
>>>>>>> master
        }
    }

    void Update() {
    }
}
