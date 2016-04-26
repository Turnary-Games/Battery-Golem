using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class NPCDialogBox : MonoBehaviour {

	public Text dialogText;
	public Text continueText;
    public Image continueIcon;
	public float wordPerSecond = 1;
	public float flashPeriod = 1;
	
	[System.NonSerialized]
	public string dialog;
	[HideInInspector]
	public Transform target;

	[Header("Audio")]
	public AudioSource playOnLetter;
	[HideInInspector]
	public float randomPitchMin = 1;
	[HideInInspector]
	public float randomPitchMax = 1;

	public bool done { get { return dialogText != null && dialog != null && dialogText.text.Length == dialog.Length; } }
	
	private float timePassed = 0;

	void Update() {
		if (!done) {
			if (wordPerSecond <= 0) {
				dialogText.text = dialog;
			} else {
				// Next letter
				timePassed += Time.deltaTime;

				int oldLength = dialogText.text.Length;
				int end = Mathf.Min(Mathf.FloorToInt(timePassed * wordPerSecond), dialog.Length);
				dialogText.text = dialog.Substring(0, end);

				if (oldLength < dialog.Length) {
					string newText = dialog.Substring(oldLength, end - oldLength);

					if (newText.Length > 0 && Regex.IsMatch(newText, "\\w") && playOnLetter && playOnLetter.clip) {
						GameObject clone = new GameObject(name + " audio");
						clone.transform.SetParent(playOnLetter.transform);
						clone.transform.position = Vector3.zero;
						clone.transform.rotation = Quaternion.identity;
						clone.transform.localScale = Vector3.one;

						AudioSource aud = clone.AddComponent<AudioSource>();

						aud.outputAudioMixerGroup = playOnLetter.outputAudioMixerGroup;
						aud.playOnAwake = false;
						aud.loop = false;
						aud.clip = playOnLetter.clip;
						aud.volume = playOnLetter.volume;
						aud.priority = playOnLetter.priority;
						aud.panStereo = playOnLetter.panStereo;
						aud.spatialBlend = playOnLetter.spatialBlend;
						aud.reverbZoneMix = playOnLetter.reverbZoneMix;
						aud.pitch = Random.Range(randomPitchMin, randomPitchMax);
						aud.bypassReverbZones = playOnLetter.bypassReverbZones;
						aud.bypassEffects = playOnLetter.bypassEffects;
						aud.bypassListenerEffects = playOnLetter.bypassListenerEffects;
						// 3D sound settings
						aud.dopplerLevel = playOnLetter.dopplerLevel;
						aud.spread = playOnLetter.spread;
						aud.rolloffMode = playOnLetter.rolloffMode;
						aud.minDistance = playOnLetter.minDistance;
						aud.maxDistance = playOnLetter.maxDistance;

						aud.Play();
						Destroy(clone, aud.clip.length);
					}
				}
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
		dialogText.text = "";
	}

	public void SkipAnimation() {
		dialogText.text = dialog;
	}

}
