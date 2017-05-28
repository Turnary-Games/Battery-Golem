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

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(-1, transform.position, transform.rotation, UnityEditor.HandleUtility.GetHandleSize(transform.position), EventType.ignore);
	}
#endif

}
