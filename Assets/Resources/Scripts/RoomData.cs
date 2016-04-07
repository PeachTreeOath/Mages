using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomData : MonoBehaviour {

    private GameObject joinGameInput;  //room name will be put here if selected from list

    private MatchDesc listData;

    public void setRec(GameObject inpTextField) {
        joinGameInput = inpTextField;
    }

    public void setData(MatchDesc d) {
        listData = d;
        joinGameInput.GetComponent<Text>().text = d.name;
    }

    public delegate void selectionHandler(BaseEventData eventData);

    public static selectionHandler handleSelection(BaseEventData d)  { Debug.Log("asdfasfas");return null; }
}
