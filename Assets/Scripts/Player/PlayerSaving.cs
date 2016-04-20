using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using Saving;

[RequireComponent(typeof(UniqueId))]
public class PlayerSaving : PlayerSubClass, ISaveable {

	int exitID = -1;

	public static void SetExitID(int exitID) {
		PlayerController.instance.saving.exitID = exitID;
	}

	public void OnSave(ref Dictionary<string, object> data) {
		data["player@exitID"] = exitID;
	}

	public void OnLoad(Dictionary<string, object> data) {
		// Goto position
		int exitID = (int)data["player@exitID"];

		
		SpawnPoint exit = SpawnPoint.GetFromID(exitID);
		if (exit) {
			transform.position = exit.transform.position;
			transform.rotation = exit.transform.rotation;
			return;
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Home)) {
			GameSaveManager.SaveRoom();
		}
	}

}
