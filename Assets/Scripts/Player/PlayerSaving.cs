using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using Saving;

[RequireComponent(typeof(UniqueId))]
public class PlayerSaving : PlayerSubClass, ISaveable {

	public static int exitID = -1;

	public void OnSave(ref Dictionary<string, object> data) {
		print("Save player");
	}

	public void OnLoad(Dictionary<string, object> data) {
		// Goto position
		GetExitPosition();

		print("Load player");
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Home)) {
			GameSaveManager.SaveRoom();
		}
	}

	void GetExitPosition() {
		if (exitID >= 0) {
			SpawnPoint exit = SpawnPoint.GetFromID(exitID);
			if (exit) {
				print("TAKE " + exit.name + " (id=" + exit.ID + "): " + exit.transform.position);
				transform.position = exit.transform.position;
				transform.rotation = exit.transform.rotation;
				return;
			}
		}

		var def = FindObjectOfType<DefaultSpawnPoint>();
		if (def) {
			print("TAKE DEFAULT: " + def.transform.position);
			transform.position = def.transform.position;
			transform.rotation = def.transform.rotation;
		}
	}

}
