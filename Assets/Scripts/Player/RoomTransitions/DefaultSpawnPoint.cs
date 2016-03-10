using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DefaultSpawnPoint : MonoBehaviour {
	
	void Awake() {
		if (!PlayerController.instance) {
			GotoRoom.lastRoom = SceneManager.GetActiveScene().name;
			RoomManager.LoadRoom("InitializingScene");
		}
	}

}
