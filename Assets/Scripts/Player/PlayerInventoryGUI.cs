using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayerInventoryGUI : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public GUISkin guiSkin;

	[Header("Settings")]

	public bool drawInEditMode;

	private string tmp = "DERP";

	void OnGUI() {
		if (!drawInEditMode && !Application.isPlaying)
			return;

		GUI.skin = guiSkin;

		tmp = GUILayout.TextField(tmp, GUILayout.Width(250));
	}

}
