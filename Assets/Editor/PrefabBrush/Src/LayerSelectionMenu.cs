using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LayerSelectionMenu
{
    [SerializeField]
    string[] _strings;
    [SerializeField]
    string[] _extraStrings;
    [SerializeField]
    int _selection = 0;

    const string _refresh = "Refresh...";

    public LayerSelectionMenu()
    {
        _extraStrings = null;
        Populate();
    }

    public LayerSelectionMenu(params string[] extra_strings)
    {
        _extraStrings = extra_strings;
        Populate(_extraStrings);
    }

    public string GetLayer()
    {
        return _strings[_selection];
    }

    public void SetSelection(string layer)
    {
        for (int i = 0; i < _strings.Length; ++i)
            if (_strings[i] == layer)
            {
                _selection = i;
                break;
            }
    }

    void Populate(params string[] extra_strings)
    {
        // store the names of each layer for slection purposes
        List<string> layer_strings = new List<string>();
        string layer = "";

        if (extra_strings != null)
        {
            // add in the requested extra names
            for (int i = 0; i < extra_strings.Length; ++i)
                layer_strings.Add(extra_strings[i]);

            // setting the default selection to the default layer
            _selection = extra_strings.Length;
        }
        else
            _selection = 0;

        // 31 = max number of layers
        for (int i = 0; i < 31; ++i)
        {
            if (layer.Length > 1)
                layer_strings.Add(layer);
            layer = LayerMask.LayerToName(i);
        }

        layer_strings.Add(_refresh);

        _strings = layer_strings.ToArray();
    }

    public string DrawGUI(Rect rect)
    {
        _selection = EditorGUI.Popup(rect, _selection, _strings);

        if (_strings[_selection] == _refresh)
            Populate(_extraStrings);

        return _strings[_selection];
    }

    public string DrawGUI()
    {
        _selection = EditorGUILayout.Popup(_selection, _strings);

        if (_strings[_selection] == _refresh)
            Populate(_extraStrings);

        return _strings[_selection];
    }
}
