using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//This is the main script for performing weapon updates.  It is referenced by the WeaponProps that uses it.
public class Weapon : NetworkBehaviour {

    private WeaponProps props; //This will be populated via code. The object must have a weapon props component.

    private bool readyToShoot = true;

    // Use this for initialization
    void Start() {
        if (isClient) {
            props = GetComponent<WeaponProps>();
            if (props == null) {
                Debug.LogError("Unable to find required WeaponProps component on Object " + gameObject.name);
            } else {
                Debug.Log("Weapon loaded for " + gameObject.name);
                props.startInit(this);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (isClient) {
            props.doUpdate();
        }
    }

    [Command] //Called by the specific weapon when it wants to fire a shot (executed on the server)
    public void CmdFire() {
        List<GameObject> thingsToSpawn = props.doFireCallback();
        foreach (GameObject go in thingsToSpawn) {
            NetworkServer.Spawn(go);
        }

    }
}
