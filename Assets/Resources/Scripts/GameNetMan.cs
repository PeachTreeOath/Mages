using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class GameNetMan : NetworkLobbyManager {

    [SerializeField]
    private GameObject roomListParent;
    [SerializeField]
    private GameObject roomTextPrefab;

    private bool singlePlayer = false;
    private bool doRefresh = false;
    private float refreshInterval = 5.0f;
    private float refreshTimeDue = 0;


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
        Debug.Log("Starting network host: isNetworkActive=" + singleton.isNetworkActive);
        singlePlayer = true;
        StartHost();
        Debug.Log("Starting network host: isNetworkActive=" + singleton.isNetworkActive);
    }

    public override void OnStartHost() {
        if(!singlePlayer) {
            base.OnStartHost();
        } else {
            //we can't call start client here or unity will bitch
            SceneManager.LoadScene(playScene); //jump to the "lobby" screen
        }
    }


    //onclick handler
    public void refreshMultiplayerRooms() {
        doRefresh = true;
        if (matchMaker == null) {
            Debug.Log("CloudID is: " + Application.cloudProjectId);
            SetMatchHost("mm.unet.unity3d.com", 443, true);
            StartMatchMaker();
        }
        refreshTimeDue = Time.time + refreshInterval;
    }

    private void doRefreshNow() {
        Debug.Log("Refreshing multiplayer room list");
        if (matchMaker == null) {
            Debug.LogError("Match maker null");
            return;
        }
        if (roomListParent == null) {
            Debug.LogError("No room list gameobject set");
            return;
        }
        ListMatchRequest listReq = new ListMatchRequest();
        listReq.nameFilter = "";
        listReq.pageNum = 1;
        listReq.pageSize = 10;
        listReq.projectId = Application.cloudProjectId;
        matchMaker.ListMatches(listReq, roomListCallback);
        //matchMaker.ListMatches(1, 10, "", roomListCallback);
    }

    public virtual void roomListCallback(ListMatchResponse resp) {
        Debug.Log("List rooms was successful: " + resp.success);  //FIXME WHYYYYYYYYYYYYYYYYYYYYYYYYYYY doesn't this return any results ever?!?!!?!?!
        Debug.Log("Found " + resp.matches.Count + " rooms");
        //cleanup existing list
        //foreach (Transform child in roomListParent.transform) {
        //Destroy(child.gameObject);
        //}
        foreach (MatchDesc d in resp.matches) {
            GameObject.Instantiate(roomTextPrefab);
            roomTextPrefab.GetComponent<Text>().text = d.name;
            roomTextPrefab.transform.SetParent(roomListParent.transform, false);
        }
        OnMatchList(resp);
    }

    public void createRoom(GameObject text) {
        string roomName = text.GetComponent<InputField>().text;
        if (roomName != null && roomName.Length > 0) {
            Debug.Log("CREATING SUPER SMALL ROOM BECAUSE WE NEED MORE CCUS");
            Debug.Log("Room name will be " + roomName);
            //matchMaker.CreateMatch(roomName, 2, true, "", createRoomCallback);
            CreateMatchRequest matchReq = new CreateMatchRequest();
            matchReq.name = roomName;
            matchReq.size = 2;
            matchReq.advertise = true;
            matchReq.password = "";
            matchMaker.CreateMatch(matchReq, createRoomCallback);
        }
    }

    public virtual void createRoomCallback(CreateMatchResponse resp) {
        Debug.Log("Room creation was successful: " + resp.success);
        if (resp.success) {
            //ideally we could continue to refresh and sit in the lobby here, but unity apparently will not allow us to create a game and not join it immediately.
            doRefresh = false;
            OnMatchCreate(resp); //this will join immediately, which may be what we want once testing is done
            Debug.Log("Joined");
            ServerChangeScene(playScene);  //is this necessary??

            Debug.Log("Scene switched");
        } else {
            doRefreshNow();
        }
    }


}
