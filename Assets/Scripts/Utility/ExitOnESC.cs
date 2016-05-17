using UnityEngine;
using System.Collections;

public class ExitOnESC : MonoBehaviour {

	public Action action;

	void Update () {
		if (Input.GetButtonDown("Quit")) {
			if (action == Action.exitGame) {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
			} else if (action == Action.openPauseMenu) {
				// Not in main menu
				if (GameSaveManager.currentRoom != 1)
					LoadingScreen.PauseGame();
			}
		}
	}

	public enum Action {
		exitGame, openPauseMenu
	}
}
