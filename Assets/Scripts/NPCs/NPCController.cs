using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class NPCController : MonoBehaviour {

	[HideInInspector]
	public List<Dialog> dialogs = new List<Dialog>();
	private int currentDialog;
	
	public Transform boxTarget;
	public GameObject prefab;
	
	[SerializeThis]
	private NPCDialogBox dialogUI;

	void OnInteractStart(PlayerController source) {
		if (!source) return;

		// Freeze the players movement
		if (source.interaction) source.interaction.talkingTo = this;
		if (source.movement) source.movement.body.velocity = Vector3.zero;

		if (dialogUI && !dialogUI.done) {
			// Skip the dialog animation
			dialogUI.SkipAnimation();
		} else {
			// Goto next dialog

			string message = GetNextMessage();

			if (message == null) {
				// Done talking
				if (dialogUI)
					Destroy(dialogUI.gameObject);
				// Release the player
				if (source.interaction) source.interaction.talkingTo = null;
			} else {
				if (dialogUI)
					// Tell the existing one to keep talking
					dialogUI.NewDialog(message);
				else
					// Create a new one
					CreateNewDialog(message);
			}
		}
	}

	void CreateNewDialog(string dialog) {
		Canvas canvas = FindObjectOfType<Canvas>();

		if (canvas) {
			// Create object from prefab
			GameObject clone = Instantiate(prefab) as GameObject;
			clone.transform.SetParent(canvas.transform);
			
			// Assign variables
			NPCDialogBox box = clone.GetComponent<NPCDialogBox>();
			box.target = boxTarget ?? transform;
			box.dialog = dialog;

			// Save for later referance
			dialogUI = box;
		} else {
			Debug.LogError("No canvas found! Please make sure one is in the scene");
		}
	}

	public Vector3 GetAxis(Vector3 from) {
		Vector3 vec = transform.position - from;
		return new Vector3(vec.x, 0, vec.z).normalized;
	}

	string GetNextMessage() {

		// Find the first dialog
		if (currentDialog == -1)
			currentDialog = dialogs.FindIndex(d => d.playOnce && d.index != -1);

		// No more playonces left
		if (currentDialog == -1)
			currentDialog = dialogs.GetRandomIndex(d => !d.playOnce);

		// Get it's string
		if (currentDialog != -1) {
			string str;

			dialogs[currentDialog] = dialogs[currentDialog].Next(out str);
			if (str == null) currentDialog = -1;

			return str;
		}

		// No other conversations
		return null;
	}

	[System.Serializable]
	public struct Dialog {
		public List<Message> messages;
		public bool playOnce;
		public int index;

		public Dialog Next(out string msg) {
			msg = null;

			// Error check
			if (index == -1) return this;
			if (index >= messages.Count) {
				index = 0;
				return this;
			}

			// Get message
			msg = messages[index].text;

			// Iterate
			index++;

			if (index >= messages.Count) {
				if (playOnce)
					index = -1;
				else
					index = messages.Count;
			}

			return this;
		}

		[System.Serializable]
		public struct Message {
			[TextArea]
			public string text;
			public bool turnHead;
		}
	}
	
}

