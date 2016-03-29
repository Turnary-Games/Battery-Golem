using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PrefabBrushEditor : EditorWindow
{
    [System.Serializable]
    class Brush
    {
        public float size = 5.0f;
        public float sizeMax = 10.0f;
        public float rate = 0.25f;
        public float timer = 0.0f;
        public bool fillMode = true;
        public Vector3 pos;
        public Vector3 dir;
        public Vector3 surfaceNorm;
    }

    class PrecisionMode
    {
        public Transform transform;
        public Vector3 scale;
        public Vector3 normal;
        public Vector3 origin;
    }

    [SerializeField]
    int _editorHash;
    bool _altDown = false;
    bool _ctrlDown = false;
    static string _defaultSaveFilePath = "Assets\\Editor\\Prefab Brush\\Data\\SaveData.txt";
    static string _brushSaveFilePath = "Assets\\Editor\\Prefab Brush\\Data\\";
    
    [SerializeField]
    Brush _brush = new Brush();

    [SerializeField]
    PrecisionMode _precision = new PrecisionMode();

    [SerializeField]
    LayerSelectionMenu _layerMenu = new LayerSelectionMenu("Nothing", "Everything");
    [SerializeField]
    string _layerSelection;

    [SerializeField]
    PrefabSelectionMenu _prefabMenu = new PrefabSelectionMenu();

    [MenuItem("Window/Prefab Brush")]

    // Window open button has been pressed
    public static void ShowWindow()
    {
        PrefabBrushEditor editor = EditorWindow.GetWindow<PrefabBrushEditor>(false, "Prefab Brush", true);
        editor.minSize = new Vector2(200, 300);

        if (File.Exists(_defaultSaveFilePath))
        {
            editor.LoadFromData(new StringReader(File.ReadAllText(_defaultSaveFilePath)));
        }
    }

    // Called when the window gets keyboard focus
    void OnFocus()
    {
        // Remove if already present and register the function
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

        // This allows us to register an update function for the scene view port
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;

        // Storing our editors hash ID for control ID purposes
        _editorHash = GetHashCode();
     }

    // Called when the window is closed
    void OnDestroy()
    {
        // Unregister our scene update function
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

        File.WriteAllText(_defaultSaveFilePath, this.ToString());
    }
    
    // Implement your window GUI here
    void OnGUI()
    {
        // Layer selection menu
        EditorGUILayout.LabelField("Layer Selection");
        _layerSelection = _layerMenu.DrawGUI();

        EditorGUILayout.Separator();

        // Brush size slider
        EditorGUILayout.LabelField("Brush Size");
        _brush.size = EditorGUILayout.Slider(_brush.size, 0.1f, _brush.sizeMax);
        _brush.sizeMax = EditorGUILayout.FloatField("Max", _brush.sizeMax);

        EditorGUILayout.Separator();

        // Fill mode toggle
        _brush.fillMode = EditorGUILayout.BeginToggleGroup("Fill Mode", _brush.fillMode);
            // Brush's rate for placing objects
            EditorGUILayout.LabelField("Brush Fill Rate");
            _brush.rate = EditorGUILayout.Slider(_brush.rate, 0.01f, 0.5f);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Separator();

        // Draw the prefab selection menu
         // First calculate the grids width (assuming cell is 145 pixels wide)
        int grid_width = (int)position.width / 145;
        _prefabMenu.DrawGUI(grid_width);

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Brush", GUILayout.Width(100)))
            SaveBrushWindow.ShowWindow();
        if (GUILayout.Button("Load Brush", GUILayout.Width(100)))
        {
            var file = EditorUtility.OpenFilePanel("Load Brush File", _brushSaveFilePath, "brush");
            if (file.Length > 0)
                LoadFromData(new StringReader(File.ReadAllText(file)));
        }
        EditorGUILayout.EndHorizontal();
    }

    // Updates whenever the scene is interacted with
    void OnSceneGUI(SceneView scene_view)
    {
        // Steps
        // 1. Shoot ray into game world from scene view
        // 2. Get point that intersects with the brush's selected layer
        // 3. Display the brush
        
        // Convert to world space
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        bool result;

        if (_layerSelection == "Everything")
            result = Physics.Raycast(ray, out hit, scene_view.camera.farClipPlane);
        else
            result = Physics.Raycast(ray, out hit, scene_view.camera.farClipPlane, 1 << LayerMask.NameToLayer(_layerSelection));

        // Find our ray's intersection through the selected layer
        if (result)
        {
            _brush.pos = hit.point;
            _brush.surfaceNorm = hit.normal;

            // Display the brush
            DisplayBrush();
        }

        // Handle the brush's input            
        HandleBrushInput(result);
    }

    // Draw the brushes circle and surface normal
    void DisplayBrush()
    {
        // Number of line segments in the circle
        int circle_segments = 30;
        
        // Draw the brush's circle
        Handles.BeginGUI();

        // Finding the direction of the brush on the surface
        _brush.dir = Vector3.Normalize(Vector3.Cross(_brush.pos, _brush.pos + _brush.surfaceNorm));
        
        Vector3 start_point =  _brush.dir * _brush.size;
        Vector3 previous_point = _brush.pos + start_point;
        Vector3 next_point;

        // Draw the brush's line segments
        for (float i = 0.0f; i < 365; i += 360.0f / circle_segments)
        {
            // Calculate the new point on the circle
            next_point = _brush.pos + Quaternion.AngleAxis(i, _brush.surfaceNorm) * start_point;
           
            // Draw a line from the old to the new
            Handles.DrawLine(HandleUtility.WorldToGUIPoint(previous_point),
                             HandleUtility.WorldToGUIPoint(next_point));

            previous_point = next_point;
        }
        
        // Draw the surface normal
        Handles.DrawLine(HandleUtility.WorldToGUIPoint(_brush.pos),
                         HandleUtility.WorldToGUIPoint(_brush.pos + _brush.surfaceNorm));
        HandleUtility.Repaint();
        Handles.EndGUI();
    }

    // Handle the brush's input
    void HandleBrushInput(bool on_surface)
    {
        // This code helps bypass the inconsistencies in Unity's GUI
        // key event handling with our own logic
        if ((Event.current.keyCode == KeyCode.LeftAlt || Event.current.keyCode == KeyCode.RightAlt))
            _altDown = (Event.current.type != EventType.KeyUp);

        if ((Event.current.keyCode == KeyCode.LeftControl || Event.current.keyCode == KeyCode.RightControl))
            _ctrlDown = (Event.current.type != EventType.KeyUp);

        if (on_surface)
        {
            int control_ID = GUIUtility.GetControlID(_editorHash, FocusType.Passive);

            if (Event.current.button == 0 && !_altDown && !_ctrlDown)
                switch (Event.current.type)
                {
                    case EventType.mouseDown:
                        if (_brush.fillMode)
                            PlacePrefab();
                        break;

                    case EventType.mouseDrag:
                        if (_brush.fillMode && Time.realtimeSinceStartup - _brush.timer >= _brush.rate)
                            PlacePrefab();
                        break;

                    case EventType.mouseUp:
                        if (!_brush.fillMode)
                        {
                            PlacePrefab();
                            Event.current.Use();
                        }
                        break;
                }
            else if (Event.current.button == 0 && _ctrlDown)
                if (Event.current.type == EventType.mouseDown)
                    PlacePrecisionPrefab();

            if (Event.current.type == EventType.Layout)
                HandleUtility.AddDefaultControl(control_ID);
        }

        Vector2 mouse_pos = Event.current.mousePosition;

        if (_precision.transform != null)
            if (Event.current.type == EventType.mouseUp ||
                mouse_pos.x > Screen.width + 50 || mouse_pos.x < -50 ||
                mouse_pos.y > Screen.height + 50 || mouse_pos.y < -50)
                ReleasePrecisionPrefab();
            else if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.Space)
                ItteratePrecisionPrefab();
            else
                UpdatePrecisionPrefab();
    }

    // Placing our randomly selected prefab into our scene in the correct layer
    void PlacePrefab()
    {
        // If we have active prefabs to place
        if (_prefabMenu.HasPrefab())
        {
            var prefab = _prefabMenu.GetPrefab();
            Vector3 position = GetRandPos();
            Quaternion rotation = prefab.GetRotation(position, _brush.surfaceNorm);

            //EDITED: this line makes sure to turn objects upside down!
            //rotation = rotation * Quaternion.AngleAxis(180, Vector3.left);

            //EDITED: here the fixed rotations are applied
            if (prefab.FixRotXYZ[0] != 0)
            {
                rotation = rotation * Quaternion.AngleAxis(prefab.FixRotXYZ[0], Vector3.left);
            }
            if (prefab.FixRotXYZ[1] != 0)
            {
                rotation = rotation * Quaternion.AngleAxis(prefab.FixRotXYZ[1], Vector3.down);
            }
            if (prefab.FixRotXYZ[2] != 0)
            {
                rotation = rotation * Quaternion.AngleAxis(prefab.FixRotXYZ[2], Vector3.back);
            }

            var obj = PrefabUtility.InstantiatePrefab(prefab.gameObject) as GameObject;
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            obj.layer = LayerMask.NameToLayer(prefab.layer);
            if (prefab.randScaleToggle)
                obj.transform.localScale *= Random.Range(prefab.randScaleMin, prefab.randScaleMax);
            obj.transform.position += _brush.surfaceNorm * prefab.depth * obj.transform.localScale.y;
            if (_prefabMenu.groupingObject)
                obj.transform.parent = _prefabMenu.groupingObject.transform;
            _brush.timer = Time.realtimeSinceStartup;
        }
    }
    
    // returns a random valid placement position
    Vector3 GetRandPos()
    {
        // Generate a random point and test to see
        // if it intersects the draw layer
        Ray ray;
        RaycastHit hit;
        bool result;
        int counter = 0;
        const int max_iterations = 30;

        do
        {
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), _brush.surfaceNorm);
            Vector3 direction = _brush.dir * Random.Range(0.0f, _brush.size);
            Vector3 position = _brush.pos + rotation * direction;

            ray = new Ray(position + _brush.surfaceNorm, -_brush.surfaceNorm);

            if (_layerSelection == "Everything")
                result = Physics.Raycast(ray, out hit, Mathf.Infinity);
            else
                result = Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer(_layerSelection));

            if (counter++ == max_iterations && !result)
            {
                hit.point = _brush.pos;
                break;
            }

        } while (!result);

        // updating the normal (for curved surfaces)
        _brush.surfaceNorm = hit.normal;

        return hit.point;
    }

    // Placing our specified precision prefab into our scene in the correct layer
    void PlacePrecisionPrefab()
    {
        // If we have active prefabs to place
        if (_prefabMenu.HasPrefab())
        {
            var prefab = _prefabMenu.GetPrefab();
            Quaternion rotation = prefab.GetRotation(_brush.pos, _brush.surfaceNorm);
            GameObject obj = Instantiate(prefab.gameObject, _brush.pos, rotation) as GameObject;

            obj.layer = LayerMask.NameToLayer(prefab.layer);
            obj.transform.position += _brush.surfaceNorm * prefab.depth;
            if (_prefabMenu.groupingObject)
                obj.transform.parent = _prefabMenu.groupingObject.transform;
            _precision.transform = obj.transform;
            _precision.scale = obj.transform.localScale;
            _precision.normal = _brush.surfaceNorm;
            _precision.origin = Event.current.mousePosition;
        }
    }

    // Update the scale and rotation of our precision prefab
    void UpdatePrecisionPrefab()
    {
        if (_precision.transform)
        {
            float x = (Event.current.mousePosition.x - _precision.origin.x) / Screen.width;
            float y = ((Event.current.mousePosition.y - _precision.origin.y) / Screen.height) * -2 + 1;

            _precision.transform.rotation = Quaternion.AngleAxis(720f * x, _precision.normal);
            _precision.transform.localScale = _precision.scale * y;
        }
    }

    // Release the precision prefab
    void ReleasePrecisionPrefab()
    {
        _precision.transform = null;
    }

    // Iterate through the prefabs
    void ItteratePrecisionPrefab()
    {
        if (_prefabMenu.HasPrefab())
        {
            var prefab = _prefabMenu.ItteratePrefabs();

            GameObject newObj = Instantiate(prefab.gameObject,
                                            _precision.transform.position,
                                            _precision.transform.rotation) as GameObject;
            DestroyImmediate(_precision.transform.gameObject);
            newObj.layer = LayerMask.NameToLayer(prefab.layer);
            newObj.transform.position += _precision.normal * prefab.depth;
            if (_prefabMenu.groupingObject)
                newObj.transform.parent = _prefabMenu.groupingObject.transform;
            _precision.transform = newObj.transform;
            _precision.scale = newObj.transform.localScale;
        }
    }

    // Return a string filled with serialization data
    public override string ToString()
    {
        string info;

        info = "Layer Selection: " + _layerSelection + "\n" +
               "Brush Max Size: " + _brush.sizeMax.ToString() + "\n" +
               "Brush Size: " + _brush.size.ToString() + "\n" +
               "Fill Mode: " + _brush.fillMode.ToString() + "\n" +
               "Fill Rate: " + _brush.rate.ToString() + "\n" +
               _prefabMenu.ToString();

        return info;
    }

    // Construct this class from serialization data
    void LoadFromData(StringReader data)
    {
        string info = data.ReadLine();
        
        // Layer Selection
        _layerMenu.SetSelection(info.Substring(17).TrimEnd('\n'));

        // Brush Max Size
        info = data.ReadLine();
        _brush.sizeMax = float.Parse(info.Substring(16).TrimEnd('\n'));

        // Brush Size
        info = data.ReadLine();
        _brush.size = float.Parse(info.Substring(12).TrimEnd('\n'));

        // File Mode
        info = data.ReadLine();
        _brush.fillMode = bool.Parse(info.Substring(11).TrimEnd('\n'));

        // Fill Rate
        info = data.ReadLine();
        _brush.rate = float.Parse(info.Substring(11).TrimEnd('\n'));

        // Getting rid of the extra new line
        info = data.ReadLine();
        // Filling our string with the prefab data
        info = data.ReadToEnd();
        // Passing the info off to our prefab menu to populate itself
        _prefabMenu.LoadFromData(new StringReader(info));
    }
}
