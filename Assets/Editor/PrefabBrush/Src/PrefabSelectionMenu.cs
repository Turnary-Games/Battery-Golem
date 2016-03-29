using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PrefabSelectionMenu
{
    [System.Serializable]
    // each prefabs information
    public class Prefab
    {
        public Rect rect;
        public Object gameObject;
        public float slider = 1.0f;
        public float percent;
        public float depth;
        public bool toggle = true;
        public bool lockOrientation = false;
        public bool randRotToggle = false;
        public bool[] randRotXYZ = new bool[3] { false, false, false };
        public bool FixRotToggle = false;
        public float[] FixRotXYZ = new float[3] { 0, 0, 0 };
        public bool randScaleToggle = false;
        public float randScaleMin = 0.1f;
        public float randScaleMax = 1.0f;
        public LayerSelectionMenu layerMenu = new LayerSelectionMenu();
        public string layer;

        // Calculate the rotation of the prefab
        public Quaternion GetRotation(Vector3 position, Vector3 surface_norm)
        {
            Quaternion surface_rot = Quaternion.identity;
            float []rand_euler = { 0.0f, 0.0f, 0.0f };

            if (!lockOrientation)
            {
                surface_rot = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, surface_norm), Vector3.Cross(Vector3.up, surface_norm));
            }

            if (randRotToggle)
            {
                for (int i = 0; i < 3; ++i)
                    if (randRotXYZ[i])
                        rand_euler[i] = Random.Range(0, 361);
            }

            return surface_rot * Quaternion.Euler(rand_euler[0], rand_euler[1], rand_euler[2]);
        }
    };

    [SerializeField]
    // list of prefab added to the menu
    List<Prefab> _prefabs = new List<Prefab>();
    int _prefabItter = 0;

    // Handle of game object we're grouping the prefabs under
    public GameObject groupingObject;
    [SerializeField]
    Dictionary<string, GameObject> _groupingObjects = new Dictionary<string, GameObject>();
    [SerializeField]
    string _groupName = "";

    [SerializeField]
    Vector2 _menuScroll;

    // Draw the GUI
    public void DrawGUI(int gridWidth)
    {
        EditorGUILayout.BeginHorizontal();

        _groupName = EditorGUILayout.TextField("Prefab Group Name", _groupName);

        if (GUILayout.Button("Set") && _groupName.Length > 0)
            if (_groupingObjects.ContainsKey(_groupName))
                groupingObject = _groupingObjects[_groupName] ? _groupingObjects[_groupName] : new GameObject(_groupName);
            else
                if (!(groupingObject = GameObject.Find(_groupName)))
                    _groupingObjects.Add(_groupName, groupingObject = new GameObject(_groupName));

        if (_groupName.Length == 0)
            groupingObject = null;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Prefab Selection");

        _menuScroll = EditorGUILayout.BeginScrollView(_menuScroll);
        EditorGUILayout.BeginHorizontal();

        // keeping track of the accumulative slider values
        float slider_whole = 0.0f;

        // for each prefab draw all of its options
        for (int i = 0; i < _prefabs.Count; ++i)
        {
            if (0 == i % gridWidth && i > 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
            }
            _prefabs[i].rect = EditorGUILayout.BeginVertical(GUILayout.Width(125));
            _prefabs[i].rect = EditorGUILayout.BeginHorizontal();
            _prefabs[i].rect = EditorGUILayout.BeginVertical();
            GUILayout.Label(AssetPreview.GetAssetPreview(_prefabs[i].gameObject), GUILayout.Width(120));
            if (GUI.Button(new Rect(_prefabs[i].rect.x + 90, _prefabs[i].rect.y + 5.0f, 20, 20), "X"))
            {
                _prefabs.RemoveAt(i--);
                continue;
            }
            if (_prefabs[i].gameObject)
            {
                Rect rect = _prefabs[i].rect;
                _prefabs[i].toggle = GUI.Toggle(new Rect(rect.x + 7.5f, rect.y + 5.0f, 50, 35), _prefabs[i].toggle, "");
                _prefabs[i].layer = _prefabs[i].layerMenu.DrawGUI(new Rect(rect.x + 5, rect.y + 100, 75, 20));
                string percent = (_prefabs[i].percent * 100).ToString();
                percent = ((percent.Length > 4) ? percent.Substring(0, 4) : percent) + "%";
                EditorGUI.LabelField(new Rect(rect.x + 79, rect.y + 100, 40, 20), percent);
            }
            EditorGUILayout.EndVertical();
            _prefabs[i].slider = GUILayout.VerticalSlider(_prefabs[i].slider, 1.0f, 0.0f, GUILayout.Height(120));
            slider_whole += _prefabs[i].gameObject ? _prefabs[i].slider : 0f;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _prefabs[i].lockOrientation = EditorGUILayout.Toggle(_prefabs[i].lockOrientation, GUILayout.Width(10));
            EditorGUILayout.LabelField("Lock Orientation", GUILayout.Width(110));
            EditorGUILayout.EndHorizontal();

            //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

            EditorGUILayout.BeginHorizontal();
            _prefabs[i].FixRotToggle = EditorGUILayout.Toggle(_prefabs[i].FixRotToggle, GUILayout.Width(10));
            EditorGUILayout.LabelField("Initial Rotation", GUILayout.Width(105));
            EditorGUILayout.EndHorizontal();
            if (_prefabs[i].FixRotToggle)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("X", GUILayout.Width(60));
                EditorGUILayout.LabelField("Y", GUILayout.Width(60));
                EditorGUILayout.LabelField("Z", GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                _prefabs[i].FixRotXYZ[0] = EditorGUILayout.FloatField(_prefabs[i].FixRotXYZ[0], GUILayout.Width(60));
                _prefabs[i].FixRotXYZ[1] = EditorGUILayout.FloatField(_prefabs[i].FixRotXYZ[1], GUILayout.Width(60));
                _prefabs[i].FixRotXYZ[2] = EditorGUILayout.FloatField(_prefabs[i].FixRotXYZ[2], GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
            }

            //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

            EditorGUILayout.BeginHorizontal();
            _prefabs[i].randRotToggle = EditorGUILayout.Toggle(_prefabs[i].randRotToggle, GUILayout.Width(10));
            EditorGUILayout.LabelField("Random Rotation", GUILayout.Width(105));
            EditorGUILayout.EndHorizontal();
            if (_prefabs[i].randRotToggle)
            {
                EditorGUILayout.BeginHorizontal();
                _prefabs[i].randRotXYZ[0] = EditorGUILayout.ToggleLeft("X", _prefabs[i].randRotXYZ[0], GUILayout.Width(25));
                _prefabs[i].randRotXYZ[1] = EditorGUILayout.ToggleLeft("Y", _prefabs[i].randRotXYZ[1], GUILayout.Width(25));
                _prefabs[i].randRotXYZ[2] = EditorGUILayout.ToggleLeft("Z", _prefabs[i].randRotXYZ[2], GUILayout.Width(25));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            _prefabs[i].randScaleToggle = EditorGUILayout.Toggle(_prefabs[i].randScaleToggle, GUILayout.Width(10));
            EditorGUILayout.LabelField("Random Scale", GUILayout.Width(105));
            EditorGUILayout.EndHorizontal();
            if (_prefabs[i].randScaleToggle)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Min", GUILayout.Width(60));
                EditorGUILayout.LabelField("Max", GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                _prefabs[i].randScaleMin = EditorGUILayout.FloatField(_prefabs[i].randScaleMin, GUILayout.Width(60));
                _prefabs[i].randScaleMax = EditorGUILayout.FloatField(_prefabs[i].randScaleMax, GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Depth", GUILayout.Width(70));
            _prefabs[i].depth = EditorGUILayout.FloatField(_prefabs[i].depth, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            _prefabs[i].gameObject = EditorGUILayout.ObjectField(_prefabs[i].gameObject, typeof(GameObject), false);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        // button for adding another prefab slot
        if (GUILayout.Button("Add \nPrefab Slot"))
            _prefabs.Add(new Prefab());

        EditorGUILayout.EndScrollView();

        // calculate and display the percentages
        if (slider_whole > 0.0f)
            for (int i = 0; i < _prefabs.Count; ++i)
                _prefabs[i].percent = _prefabs[i].slider / slider_whole;
        else
            for (int i = 0; i < _prefabs.Count; ++i)
                _prefabs[i].percent = 0.0f;
    }

    // Determine if we have active prefabs available
    public bool HasPrefab()
    {
        if (_prefabs.Count == 0)
            return false;

        for (int i = 0; i < _prefabs.Count; ++i)
            if (_prefabs[i].toggle && _prefabs[i].gameObject && _prefabs[i].percent > 0.0f)
                return true;

        return false;
    }

    // Return a random valid prefab
    public Prefab GetPrefab()
    {
        var valid = new List<ProportionValue<Prefab>>();

        foreach (Prefab prefab in _prefabs)
            if (prefab.toggle && prefab.percent > 0.0f && prefab.gameObject)
                valid.Add(ProportionValue.Create(prefab.percent, prefab));

        return valid.ChooseByRandom();
    }

    // Return the next prefab in line
    public Prefab ItteratePrefabs()
    {
        if (_prefabItter < _prefabs.Count)
            return _prefabs[_prefabItter++];
        else
            return _prefabs[(_prefabItter = 0)];
    }

    // Return a string filled with serialization data
    public override string ToString()
    {
        StringWriter writer = new StringWriter();

        foreach (Prefab prefab in _prefabs)
        {
            string info = "\nGame Object: " + (prefab.gameObject != null ? AssetDatabase.GetAssetPath(prefab.gameObject.GetInstanceID()) : "None") + "\n" +
                          "Layer: " + prefab.layer + "\n" +
                          "Toggle: " + prefab.toggle.ToString() + "\n" +
                          "Slider: " + prefab.slider.ToString() + "\n" +
                          "Lock Orientation Toggle: " + prefab.lockOrientation.ToString() + "\n" +
                          "Random Rotation Toggle: " + prefab.randRotToggle.ToString() + "\n" +
                          "Random Rotation X: " + prefab.randRotXYZ[0].ToString() + "\n" +
                          "Random Rotation Y: " + prefab.randRotXYZ[1].ToString() + "\n" +
                          "Random Rotation Z: " + prefab.randRotXYZ[2].ToString() + "\n" +

                          "Random Scale Toggle: " + prefab.randScaleToggle.ToString() + "\n" +
                          "Random Scale Min: " + prefab.randScaleMin.ToString() + "\n" +
                          "Random Scale Max: " + prefab.randScaleMax.ToString() + "\n" +
                          
                    "Fixed Rotation Toggle: " + prefab.FixRotToggle.ToString() + "\n" +
                    "Fixed Rotation X: " + prefab.FixRotXYZ[0].ToString() + "\n" +
                    "Fixed Rotation Y: " + prefab.FixRotXYZ[1].ToString() + "\n" +
                    "Fixed Rotation Z: " + prefab.FixRotXYZ[2].ToString() + "\n" +
                    "Depth: " + prefab.depth.ToString() + "\n";
            writer.Write(info);
        }

        if (_groupingObjects.Count > 0)
        {
            writer.Write("\nGroup Names:\n");

            foreach (string key in _groupingObjects.Keys)
                writer.Write(key + "\n");
        }
    
        return writer.ToString();
    }

    // Construct this class from serialization data
    public void LoadFromData(StringReader data)
    {
        _prefabs.Clear();
        string info;

        while((info = data.ReadLine()) != null && info.Contains("Game Object:"))
        {
            Prefab prefab = new Prefab();

            // loading the GameObject
            prefab.gameObject = AssetDatabase.LoadAssetAtPath<Object>(info.Substring(13).TrimEnd('\n'));

            // Setting Layer
            info = data.ReadLine();
            prefab.layerMenu.SetSelection(info.Substring(7).TrimEnd('\n'));

            // Setting toggle
            info = data.ReadLine();
            prefab.toggle = bool.Parse(info.Substring(8).TrimEnd('\n'));

            // Setting slider value
            info = data.ReadLine();
            prefab.slider = float.Parse(info.Substring(8).TrimEnd('\n'));

            // Setting lock orientation toggle
            info = data.ReadLine();
            prefab.lockOrientation = bool.Parse(info.Substring(25).TrimEnd('\n'));

            // Setting random rotation toggle
            info = data.ReadLine();
            prefab.randRotToggle = bool.Parse(info.Substring(24).TrimEnd('\n'));

            // Setting random XYZ toggles
            for (int i = 0; i < 3; ++i)
            {
                info = data.ReadLine();
                prefab.randRotXYZ[i] = bool.Parse(info.Substring(19).TrimEnd('\n'));
            }

            // Setting random scale toggle
            info = data.ReadLine();
            prefab.randScaleToggle = bool.Parse(info.Substring(21).TrimEnd('\n'));

            // Setting random scale min / max
            info = data.ReadLine();
            prefab.randScaleMin = float.Parse(info.Substring(18).TrimEnd('\n'));
            info = data.ReadLine();
            prefab.randScaleMax = float.Parse(info.Substring(18).TrimEnd('\n'));

            // Setting FIXED rotation toggle
            info = data.ReadLine();
            prefab.FixRotToggle = bool.Parse(info.Substring(22).TrimEnd('\n'));


            // Setting FIXED XYZ toggles
            for (int i = 0; i < 3; ++i)
            {
                info = data.ReadLine();
                prefab.FixRotXYZ[i] = float.Parse(info.Substring(18).TrimEnd('\n'));
            }

            // Setting depth
            info = data.ReadLine();
            prefab.depth = float.Parse(info.Substring(6).TrimEnd('\n'));



            // Adding the newly constructed prefab
            _prefabs.Add(prefab);

            // getting rid of the extra newline
            info = data.ReadLine();
        }

        _groupingObjects.Clear();
        while((info = data.ReadLine()) != null)
        {
            GameObject obj;
            if ((obj = GameObject.Find(info)) && !_groupingObjects.ContainsKey(info))
                _groupingObjects.Add(info, obj);
        }
    }
}
