using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class RoomTransitionPoint : MonoBehaviour {

	[SceneDropDown]
	public string gotoRoomOnTrigger;
	public int exitID = -1;
	public bool bringPlayer = true;

	bool works = false;

	void OnTriggerEnter(Collider col) {
		if (LevelSerializer.IsDeserializing) return;
		if (RoomManager.loadingRoom) return;
		if (!works) return;

		GameObject main = col.GetMainObject();
		if (main.tag == "Player") {
			if (bringPlayer)
				PlayerController.instance.exitID = exitID;
			else
				Destroy(PlayerController.instance.transform.root.gameObject);

			// Spawn loading screen
			LoadingScreen.LoadRoom(gotoRoomOnTrigger);

			//RoomManager.LoadRoom(gotoRoomOnTrigger);
		}
	}

	IEnumerator WAIT() {
		yield return new WaitForFixedUpdate();
		works = true;
	}

	void OnRoomWasLoaded() {
		StartCoroutine(WAIT());
	}
}
