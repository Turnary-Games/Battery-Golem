using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogBox : MonoBehaviour {

	public Text dialogText;
	public Text continueText;
    public Image continueIcon;
	public float wordPerSecond = 1;
	public float flashPeriod = 1;

	[HideInInspector]
	public string dialog;
	[HideInInspector]
	public Transform target;

	public bool done { get { return dialogText.text.Length == dialog.Length; } }

	[SerializeThis]
	private float timePassed = 0;

	void Update() {
		if (!done) {
			if (wordPerSecond <= 0) {
				dialogText.text = dialog;
			} else {
				timePassed += Time.deltaTime;
				dialogText.text = dialog.Substring(0, Mathf.Min(Mathf.FloorToInt(timePassed * wordPerSecond), dialog.Length));
			}
		}

		// Flash the continue text
		//Color c = continueText.color;
        Color c = continueIcon.color;

        float B = 2 * Mathf.PI / flashPeriod;

		c.a = -Mathf.Cos(B * Time.time) / 2 + .5f;

		//continueText.color = c;
        continueIcon.color = c;

    }

	void LateUpdate() {
		if (target)
			transform.position = Camera.main.WorldToScreenPoint(target.position);
	}

	public void NewDialog(string dialog) {
		this.dialog = dialog;
		timePassed = 0;
	}

	public void SkipAnimation() {
		dialogText.text = dialog;
	}

}
