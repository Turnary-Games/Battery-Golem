using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCController : MonoBehaviour {

	[TextArea]
	public string dialog;
	public GameObject prefab;
	[HideInInspector]
	public NPCDialogBox current;

	void OnInteractStart() {
		Canvas canvas = FindObjectOfType<Canvas>();

		if (canvas) {

			GameObject clone = Instantiate(prefab) as GameObject;
			clone.transform.SetParent(canvas.transform);

			NPCDialogBox box = clone.GetComponent<NPCDialogBox>();
			box.position = transform.position;
			box.dialog = dialog;

			if (current)
				current.prev = box;

			current = box;
		} else {
			Debug.LogError("No canvas found! Please make sure one is in the scene!");
		}
	}

}
