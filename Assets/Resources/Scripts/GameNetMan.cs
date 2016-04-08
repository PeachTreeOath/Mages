using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.EventSystems;

public class GameNetMan : NetworkLobbyManager {

    [SerializeField]
    private GameObject roomListParent;
    [SerializeField]
    private GameObject roomTextPrefab;
    [SerializeField]
    private GameObject joinRoomInput;  //Type InputField, obviously view should be separated out here...but, uh, not my game
                                       //[SerializeField]
                                       //private GameObject joinRoomBut;

    private bool singlePlayer = false;
    private bool doRefresh = false;
    private float refreshInterval = 2.0f;
    private float refreshTimeDue = 0;


    // Use this for initialization
    void Start() {
        if(joinRoomInput == null || joinRoomInput.GetComponentInChildren<Text>() == null) {
            Debug.LogError("No join room input field assigned to GameNetMan.  Shit's weak");
        }

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

    public void stopMultiplayer() {
        doRefresh = false;
        StopHost(); //This should stop server, client, and matchmaker
    }

    public void exitGame() {
        Destroy(gameObject);
        Shutdown();
        Debug.Log("YOLO");
        Application.Quit();
    }

    //onclick handler
    public void refreshMultiplayerRooms() {
        doRefresh = true;
        if (matchMaker == null) {
            Debug.Log("CloudID is: " + Application.cloudProjectId);
            SetMatchHost("mm.unet.unity3d.com", 443, true);
            StartMatchMaker();
            doRefreshNow();
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
        listReq.pageNum = 0;
        listReq.pageSize = 10;
        listReq.projectId = Application.cloudProjectId;
        matchMaker.ListMatches(listReq, roomListCallback);
    }

    public virtual void roomListCallback(ListMatchResponse resp) {
        if(!doRefresh) {
            return; //refreshes halted
        }
        Debug.Log("Found " + resp.matches.Count + " rooms");
        matches = resp.matches;
        //cleanup existing list
        foreach (Transform child in roomListParent.transform) {
        Destroy(child.gameObject);
        }
        foreach (MatchDesc d in resp.matches) {
            GameObject go = GameObject.Instantiate(roomTextPrefab);
            go.GetComponentInChildren<Text>().text = d.name + "\t(" + d.currentSize + "/" + d.maxSize + ")" ; //may be sub-sub component
            go.transform.SetParent(roomListParent.transform, false); //Just parent the object to the panel and let the panel lay it out

            //setup data for the selection obj
            RoomData rd = go.AddComponent<RoomData>();
            rd.setReceiver(joinRoomInput);
            rd.setData(d);

        }
        OnMatchList(resp); //maybe unnecessary
    }

    public void createRoom(GameObject text) {
        string roomName = text.GetComponent<InputField>().text;
        if (roomName != null && roomName.Length > 0) {
            Debug.Log("CREATING SUPER SMALL ROOM BECAUSE WE NEED MORE CCUS");
            Debug.Log("Room name will be " + roomName);
            //matchMaker.CreateMatch(roomName, 2, true, "", createRoomCallback); //we want moar control
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
            doRefreshNow();
            doRefresh = false;
            OnMatchCreate(resp); //this will join immediately, which may be what we want once testing is done
            ServerChangeScene(playScene);  //is this necessary??
            Debug.Log("Joined");

            Debug.Log("Scene switched");
        } else {
            doRefreshNow();
        }
    }

    public void joinGame(GameObject text) {
        string roomName = text.GetComponent<InputField>().text;
        if (roomName != null && roomName.Length > 0) {
            Debug.Log("Joining room named " + roomName);
            //find the info for the given room
            MatchDesc j = null;
            foreach(MatchDesc d in matches) {
                if (roomName.Equals(d.name)){
                    j = d;
                    break;
                }
            }
            if(j == null) {
                Debug.LogError("Match not found: " + roomName);
                return;
            }
            JoinMatchRequest matchReq = new JoinMatchRequest();
            matchReq.networkId = j.networkId;
            matchReq.password = "";
            matchMaker.JoinMatch(matchReq, joinGameCallback);
        }
    }

    public virtual void joinGameCallback(JoinMatchResponse resp) {
        Debug.Log("Join response: " + resp.ToString());
        if(resp.success == true) {
            Debug.Log("JOIN OK");
            doRefresh = false;
            OnMatchJoined(resp);
            ServerChangeScene(playScene);  //is this necessary??
        }
    }


    //This should intercept and do nothing so the bastard lobby code won't try to doubley start the game
    public override void OnClientSceneChanged(NetworkConnection conn) {
        Debug.Log("Intercepting lobby scene change handler");
        //base.OnClientSceneChanged(conn);
    }


}
