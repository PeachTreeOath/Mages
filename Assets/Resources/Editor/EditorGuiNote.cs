using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Note))]
public class EditorGuiNote : Editor {
    private SerializedProperty noteContent;
    private bool editable = false;
    private GUILayoutOption[] options = {
        GUILayout.ExpandWidth (true),
        GUILayout.ExpandHeight (false)
    };

    private void OnEnable() {
        noteContent = serializedObject.FindProperty("note");
        editable = false;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.GetStyle("textArea"));
        textAreaStyle.wordWrap = true;
        textAreaStyle.stretchWidth = true;
        textAreaStyle.fixedHeight = 0;
        textAreaStyle.stretchHeight = true;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        labelStyle.wordWrap = true;
        labelStyle.stretchWidth = true;
        labelStyle.fontStyle = FontStyle.Normal;

        GUIStyle labelBoldStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        labelBoldStyle.fontStyle = FontStyle.Bold;

        if (GUILayout.Button(editable ? "Lock" : "Edit")) {
            editable = !editable;
        }

        if (editable) {
            GUILayout.Label("Notes", labelBoldStyle);
            noteContent.stringValue = EditorGUILayout.TextArea(noteContent.stringValue,
                textAreaStyle,
                options);
            if(noteContent.stringValue.Length == 0) {
                noteContent.stringValue = "(none)";
            }
        } else {
            GUILayout.Label("Notes", labelBoldStyle);
            EditorGUILayout.LabelField(noteContent.stringValue,
                labelStyle,
                options);
        }

        serializedObject.ApplyModifiedProperties();
    }
}