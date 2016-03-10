using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour {

	[HideInInspector] [TextArea]
	public List<string> dialog = new List<string>();
	public Transform boxTarget;
	public GameObject prefab;
	
	[SerializeThis]
	private NPCDialogBox current;

	[SerializeThis]
	private int index = -1;

	void OnInteractStart(PlayerController source) {
		if (!source) return;

		// Freeze the players movement
		if (source.interaction) source.interaction.talkingTo = this;
		if (source.movement) source.movement.body.velocity = Vector3.zero;

		if (!current || current.done) {
			// Goto next dialog
			index++;

			if (index >= dialog.Count) {
				// Done talking
				index = -1;
				Destroy(current.gameObject);
				// Release the player
				if (source.interaction) source.interaction.talkingTo = null;
			} else {
				if (current) {
					current.NewDialog(dialog[index]);

				} else CreateNewDialog(dialog[index]);
			}
		} else {
			// Skip the dialog animation
			current.SkipAnimation();
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
			current = box;
		} else {
			Debug.LogError("No canvas found! Please make sure one is in the scene");
		}
	}

	public Vector3 GetAxis(Vector3 from) {
		Vector3 vec = transform.position - from;
		return new Vector3(vec.x, 0, vec.z).normalized;
	}

}
