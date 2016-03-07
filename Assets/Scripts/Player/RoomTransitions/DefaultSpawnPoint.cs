using UnityEngine;
using System.Collections;

public class DefaultSpawnPoint : MonoBehaviour {
	
	void Awake() {
		if (!PlayerController.instance) {
			RoomManager.LoadRoom("InitializingScene");
		}
	}

}
