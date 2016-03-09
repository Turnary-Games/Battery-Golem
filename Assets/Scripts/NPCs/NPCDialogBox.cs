using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogBox : MonoBehaviour {

	public Text ui;
	public float wordPerSecond = 1;
	[HideInInspector]
	public string dialog;
	[HideInInspector]
	public Vector3 position;
	[HideInInspector]
	public NPCDialogBox prev;

	[SerializeThis]
	private float timePassed = 0;

	void Update() {
		if (ui.text.Length != dialog.Length) {
			if (wordPerSecond <= 0) {
				ui.text = dialog;
			} else {
				timePassed += Time.deltaTime;
				ui.text = dialog.Substring(0, Mathf.Min(Mathf.FloorToInt(timePassed * wordPerSecond), dialog.Length));
			}
		}
	}

	void LateUpdate() {
		if (prev) {

			transform.localPosition = prev.transform.localPosition + Vector3.up * prev.GetComponent<RectTransform>().sizeDelta.y;
		} else {
			transform.position = Camera.main.WorldToScreenPoint(position);
		}
	}

}
