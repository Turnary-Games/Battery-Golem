using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class SaveBrushWindow : EditorWindow {

    string _brushName;
    static string _saveFilePath = "Assets\\Editor\\Prefab Brush\\Data\\";

	public static void ShowWindow()
    {
        var editor = EditorWindow.GetWindow<SaveBrushWindow>(true, "Save Brush", true);
        editor.maxSize = new Vector2(270, 30);
        editor.minSize = new Vector2(270, 30);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _brushName = EditorGUILayout.TextField("Brush Name:", _brushName);
        if (GUILayout.Button("Save"))
        {
            PrefabBrushEditor editor = EditorWindow.GetWindow<PrefabBrushEditor>();
            File.WriteAllText(_saveFilePath + _brushName + ".brush", editor.ToString());
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}
