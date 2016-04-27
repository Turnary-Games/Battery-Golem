using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResetSaves : MonoBehaviour {
	
	void Awake () {
		ResetAllRooms();
	}

	public static void ResetAllRooms() {
		GameSaveManager.roomData = new Dictionary<string, Dictionary<string, object>>();
	}
}
