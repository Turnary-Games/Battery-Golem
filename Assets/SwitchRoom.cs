using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class SwitchRoom : MonoBehaviour {

	[SceneDropDown]
	public string nextRoom;

	[SerializeThis]
	private bool wait;

	void OnTriggerEnter(Collider other) {
		if (other.GetMainObject().tag == "Player") {
			print("Load room \"" + nextRoom + "\"");
			wait = true;
			RoomManager.LoadRoom(nextRoom);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.GetMainObject().tag == "Player") {
			print("Player left");
			wait = false;
		}
	}
}
