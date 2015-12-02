using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	public TextMesh textMesh;
	public float fadeTime;

	private float fade; // 0 to 1
	private bool stay = true;

	void LateUpdate() {
		if (fadeTime != 0) {
			fade += Time.deltaTime / fadeTime * (stay ? 1 : -1);
			fade = Mathf.Clamp(fade, 0f, 1f);
		} else
			fade = stay ? 1 : 0;

		transform.localScale = Vector3.one * fade;

		if (fade == 0 && !stay)
			Destroy(gameObject);

		stay = false;
	}

	public void Keep() {
		stay = true;
	}
}
