using UnityEngine;
using System.Collections;

public class ExitOnESC : MonoBehaviour {

	void Update () {
		if (Input.GetButtonDown("Quit")) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
