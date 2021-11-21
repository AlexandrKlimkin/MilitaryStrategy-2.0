using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        var dungeonGenerator = (DungeonGenerator)target;
        base.OnInspectorGUI();
        if (GUILayout.Button(new GUIContent { text = "GENERATE" })) {
            dungeonGenerator.GenerateDungeon();
        }
        if (GUILayout.Button(new GUIContent { text = "CLEAR" })) {
            dungeonGenerator.ClearDungeon();
        }
        if (GUILayout.Button(new GUIContent { text = "TRIANGULATE" })) {
            dungeonGenerator.Triangulate();
        }
    }
}
