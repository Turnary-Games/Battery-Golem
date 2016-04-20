using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class RoomTransitionPoint : MonoBehaviour {

	[SceneDropDown]
	public string gotoRoomOnTrigger;
	public int exitID = -1;
	public bool bringPlayer = true;

	bool works = false;

	void Awake() {
		StartCoroutine(WAIT());
	}

	void OnTriggerEnter(Collider col) {
		if (!works) return;

		GameObject main = col.GetMainObject();
		if (main.tag == "Player") {
			works = false;

			if (bringPlayer)
				PlayerSaving.exitID = exitID;
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
	
}
