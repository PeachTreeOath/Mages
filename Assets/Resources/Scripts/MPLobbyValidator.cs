using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MPLobbyValidator : MonoBehaviour {

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private Button createButton;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void createRoomInput(GameObject textField) {
        string value = textField.GetComponent<InputField>().text;
        if (value != null && value.Length > 0) {
            createButton.interactable = true;
        } else {
            createButton.interactable = false;
        }
    }

    public void joinRoomInput(GameObject textField) {
        string value = textField.GetComponent<InputField>().text;
        if (value != null && value.Length > 0) {
            joinButton.interactable = true;
        } else {
            joinButton.interactable = false;
        }
    }
}
