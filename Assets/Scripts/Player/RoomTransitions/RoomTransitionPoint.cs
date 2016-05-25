using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;

public class RoomTransitionPoint : MonoBehaviour {

	[SceneDropDown]
	public int gotoRoomOnTrigger = -1;
	public int exitID = -1;
	public bool openCredits;

	[HideInInspector]
	public bool filter;
	[HideInInspector]
	public int idMustBe = -1;

	bool works = false;

	void Awake() {
		StartCoroutine(WAIT());
	}

	void OnTriggerEnter(Collider col) {
		if (!works) return;

		GameObject main = col.GetMainObject();
		if (main.tag == "Player") {
			if (filter && PlayerController.instance.movement.autoMoveID != idMustBe) return;
			
			works = false;

			PlayerSaving.SetExitID(exitID);

			// Spawn loading screen
			GameSaveManager.SaveRoom();
			if (openCredits) {
				// Load with a delegate callback
				LoadingScreen.LoadRoom(gotoRoomOnTrigger, ls => {
					// Loaded callback
					var buttonActions = FindObjectOfType<ButtonActions>();
					if (buttonActions)
						buttonActions.OpenCreditsMenu();
				});
				UnityEngine.Analytics.Analytics.CustomEvent("completeGame", new Dictionary<string, object> {
					{ "playTime", Time.time },
					{ "version", BatteryGolemVersion.FormatVersion(BatteryGolemVersion.CURRENT) }
				});
			} else {
				// Load normally
				LoadingScreen.LoadRoom(gotoRoomOnTrigger);
			}
		}
	}

	IEnumerator WAIT() {
		yield return new WaitForFixedUpdate();
		works = true;
	}
	
}
