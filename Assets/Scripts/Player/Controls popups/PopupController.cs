using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupController : MonoBehaviour {

	public PlayerController player;
	public Vector3 offset = Vector3.up * 3f;
	public GameObject prefab;

	[Header("Text messages")]
	public string interact = "[E]";
	public string electrify = "[E]";
	public string pickup = "[Q]";

	private List<Popup> popups = new List<Popup>();

	public void AddPopupFocusing(Transform target, Type type) {
		var clone = Instantiate(prefab, target.position, target.rotation) as GameObject;
		clone.transform.SetParent(target);

		InitiatePopup(clone, type);
	}

	public void AddPopupAt(Vector3 position, Type type) {
		var clone = Instantiate(prefab, position, Quaternion.identity) as GameObject;

		InitiatePopup(clone, type);
	}

	private void InitiatePopup(GameObject clone, Type type) {
		var textMesh = clone.GetComponent<TextMesh>();

		if (type == Type.interact)
			textMesh.text = interact;
		if (type == Type.electrify)
			textMesh.text = electrify;
		if (type == Type.pickup)
			textMesh.text = pickup;

		clone.transform.position += offset;
	}

	public enum Type {
		interact, electrify, pickup
	}

}
