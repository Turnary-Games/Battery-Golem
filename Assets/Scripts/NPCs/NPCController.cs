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

	void Start() {
		string test = null;
		print(test);
	}

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
		int index = dialogs.FindIndex(d => d.playOnce);
		if (index != -1) {
			if (dialogs[index].done) {
				dialogs.RemoveAt(index);
				return null;
			} else
				return dialogs[index].Next();
		}

		// No more playonces left
		if (currentDialog == -1) {
			currentDialog = dialogs.GetRandomIndex();
		}
		if (currentDialog != -1) {
			return dialogs[currentDialog].Next();
		}

		// No other conversations
		return null;
	}

	[System.Serializable]
	public struct Dialog {
		[TextArea]
		public List<string> messages;
		public bool playOnce;
		public int index;

		public bool done { get { return index == -1 || index >= messages.Count; } }

		public string Next() {
			// Error check
			if (index == -1) return null;
			if (index >= messages.Count) index = 0;

			// Get message
			string msg = messages[index];

			// Iterate
			index++;

			if (index >= messages.Count)
				if (playOnce)
					index = -1;

			return msg;
		}
	}
	
}

