using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Receives a spawned game object from the network and then repositions it
public class SpawnDelegate : MonoBehaviour {

    [SerializeField]
    List<GameObject> playerSpawns;

    [SerializeField]
    List<GameObject> enemySpawns;

    private static SpawnDelegate instance;

    void Awake() {
        instance = this;
    }

    public static SpawnDelegate getInstance() {
        return instance;
    }

    public GameObject getPlayerSpawnLocation() {
        //Get first for now
        if(playerSpawns.Count > 0) {
            return playerSpawns[0];
        }
        Debug.Log("No player spawn points, returning empty");
        return new GameObject(); //empty
    }
}
