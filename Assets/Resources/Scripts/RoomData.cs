using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class RoomData : MonoBehaviour, ISelectHandler {

    private GameObject joinGameInput;  //room name will be put here if selected from list, type is InputField

    private MatchDesc listData;

    public void setReceiver(GameObject inpTextField) {
        joinGameInput = inpTextField;
    }

    public void setData(MatchDesc d) {
        listData = d;
    }

    public void OnSelect(BaseEventData eventData) {
        if(listData != null) {
            Debug.Log("Selected room " + listData.name);
            Debug.Log("joinGame: " + joinGameInput.GetType().Name);
            Debug.Log("joinGameparent: " + joinGameInput.transform.parent.name);
            joinGameInput.GetComponentInChildren<InputField>().text= listData.name;
        }
    }
}
