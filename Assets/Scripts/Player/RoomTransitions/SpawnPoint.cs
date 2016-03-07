using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class SpawnPoint : MonoBehaviour {

	public int ID = -1;

	public static SpawnPoint GetFromID(int id) {
		foreach (SpawnPoint exit in FindObjectsOfType<SpawnPoint>()) {
			if (exit.ID == id)
				return exit;
		}

		return null;
	}

}
