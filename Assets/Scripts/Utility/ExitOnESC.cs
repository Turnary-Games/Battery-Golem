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
			} else if (action == Action.pause || action == Action.pauseAndUnpause) {
				// Not in the main menu
				if (GameSaveManager.currentRoom == 1) return;

				bool isPaused = Mathf.Approximately(Time.timeScale, 0);
				
				if (!isPaused)
					LoadingScreen.PauseGame();
				else if (action == Action.pauseAndUnpause) {
					PauseWhenAlive.Unpause();
				}
			}
		}
	}

	public enum Action {
		exitGame, pause, pauseAndUnpause
	}
}
