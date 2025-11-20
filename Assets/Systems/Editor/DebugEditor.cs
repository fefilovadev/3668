using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugManager))]
public class DebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugManager manager = (DebugManager)target;
        if (GUILayout.Button("RESET GAME"))
        {
            if (EditorUtility.DisplayDialog("Reset Game", "Are you sure you want to reset all game data?", "Yes", "No"))
            {
                manager.ResetGame();
                Debug.Log("Game reset complete.");
            }
        }
    }
}