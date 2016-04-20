using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResetSaves : MonoBehaviour {
	
	void Awake () {
		Reset();
	}

	public static void Reset() {
		//LevelSerializer.ClearCheckpoint();
		//RoomManager.rooms = new Dictionary<string, string>();
	}
}
