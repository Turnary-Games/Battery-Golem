using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonActions : MonoBehaviour {

	/// <summary>
	/// Use UnitySerialization-NG RoomManager to jump to a scene.
	/// </summary>
	public void JumpToRoom(string name) {
		RoomManager.LoadRoom(name);
	}

	/// <summary>
	/// Use Unity's built in SceneManager to jump to a scene.
	/// </summary>
	public void JumpToScene(string name) {
		SceneManager.LoadScene(name);
	}

	public void ExitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
