using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class LobbyMan : MonoBehaviour {

    [SerializeField]
    private GameObject roomListParent;
    [SerializeField]
    private GameObject roomTextPrefab;
    private ListMatchResponse lastResp;

    private NetworkManager netMan;
    private NetworkMatch mm;

    private bool doRefresh = false;
    private float refreshInterval = 3.0f;
    private float refreshTimeDue = 0;


    void Awake() {
        netMan = gameObject.GetComponent<GameNetMan>();
        if (netMan == null) {
            Debug.LogError("Unable to find NetworkManager...you are going to have a bad day");
        } else {
            Debug.Log("NetworkManager started");
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (doRefresh && Time.time > refreshTimeDue) {
            doRefreshNow();
            refreshTimeDue = Time.time + refreshInterval;
        }
    }

    //onclick handler
    public void startSinglePlayer() {
        Debug.Log("Starting network host");
        netMan.StartHost();
    }

    //onclick handler
    public void refreshMultiplayerRooms() {
        doRefresh = true;
        if (mm == null) {
            Debug.Log("CloudID is: " + Application.cloudProjectId);
            netMan.SetMatchHost("mm.unet.unity3d.com", 443, true);
            netMan.StartMatchMaker();
            mm = netMan.matchMaker;
        }
        refreshTimeDue = Time.time + refreshInterval;
    }

    private void doRefreshNow() {
        Debug.Log("Refreshing multiplayer room list");
        if (mm == null) {
            Debug.LogError("Match maker null");
            return;
        }
        if (roomListParent == null) {
            Debug.LogError("No room list gameobject set");
            return;
        }
        mm.ListMatches(1, 10, "", roomListCallback);
    }

    public virtual void roomListCallback(ListMatchResponse resp) {
        Debug.Log("Found " + resp.matches.Count + " rooms");
        lastResp = resp;
        //cleanup existing list
        foreach (Transform child in roomListParent.transform) {
            Destroy(child.gameObject);
        }
        foreach (MatchDesc d in resp.matches) {
            GameObject.Instantiate(roomTextPrefab);
            roomTextPrefab.GetComponent<Text>().text = d.name;
            roomTextPrefab.transform.SetParent(roomListParent.transform, false);
        }
    }

    public void createRoom(GameObject text) {
        string roomName = text.GetComponent<InputField>().text;
        if(roomName != null && roomName.Length > 0) {
            Debug.Log("CREATING SUPER SMALL ROOM BECAUSE WE NEED MORE CCUS");
            mm.CreateMatch(roomName, 2, true, "", createRoomCallback);
        }
    }

    public virtual void createRoomCallback(CreateMatchResponse resp) {
        Debug.Log("Room creation was successful: " + resp.success);
        doRefreshNow();
        //netMan.OnMatchCreate(resp);
    }


}
