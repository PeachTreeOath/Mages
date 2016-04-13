using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections;

//This hook will start us in the correct scene when the Editor Play button is pushed
//regardless of what scene is actually open in the editor.
[InitializeOnLoad]
public static class EditorStartInLobby {
	
    [SerializeField]
    public static string startScene = "Offline";

    private static bool off = true; //start in nonplaying mode

    static EditorStartInLobby() {
        EditorApplication.playmodeStateChanged += StateChange;
    }

    static void StateChange() {
        if (!EditorApplication.isPaused) {
            if (EditorApplication.isPlaying) {
                if (off) {
                    //We're in playmode, right after having pressed Play
                    if(!SceneManager.GetActiveScene().name.Equals(startScene)){
                        Debug.Log("***EDITOR*** Switching to start scene");
                        //SceneManager.LoadScene(startScene);
                    }
                    off = false;
                } else {
                    //now switching back to editor mode
                    off = true;
                }
            }
        }
    }
}
