using UnityEngine;
using System.Collections;

public class PauseWhenAlive : MonoBehaviour {

	void Awake() {
		Time.timeScale = 0;
	}

	void OnDestroy() {
		Time.timeScale = 1;
	}

}
