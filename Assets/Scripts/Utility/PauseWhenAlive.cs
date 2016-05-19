using UnityEngine;
using System.Collections;

public class PauseWhenAlive : MonoBehaviour {

	void Awake() {
		Time.timeScale = 0;
	}

	void OnDestroy() {
		Time.timeScale = 1;
	}

	public static void Unpause() {
		foreach (var x in FindObjectsOfType<PauseWhenAlive>()) {
			Destroy(x.gameObject);
		}
	}

}
