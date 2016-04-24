using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class RoomTransitionPoint : MonoBehaviour {

	[SceneDropDown]
	public int gotoRoomOnTrigger = -1;
	public int exitID = -1;

	bool works = false;

	void Awake() {
		StartCoroutine(WAIT());
	}

	void OnTriggerEnter(Collider col) {
		if (!works) return;

		GameObject main = col.GetMainObject();
		if (main.tag == "Player") {
			works = false;

			PlayerSaving.SetExitID(exitID);

			// Spawn loading screen
			GameSaveManager.SaveRoom();
			LoadingScreen.LoadRoom(gotoRoomOnTrigger);
		}
	}

	IEnumerator WAIT() {
		yield return new WaitForFixedUpdate();
		works = true;
	}
	
}
