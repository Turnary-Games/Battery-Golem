using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HatchPuzzle : MonoBehaviour {

	public RotateEffect leadingCog;
	public Object[] affected;

	public float fullSpeedAfter = 1;

	private float t1, t2;

	private Vector3 leadingMotion;
	private List<Vector3> motion = new List<Vector3>();

	private bool cogInPlace = false;
	private bool elec = false;

	void Awake() {
		// Save their motion
		leadingMotion = leadingCog.velocity;
		SetMotion(leadingCog, Vector3.zero, 0);

		for(int i=0; i<affected.Length; i++) {
			Object obj = affected[i];

			if (obj is ScrollTexture) {
				motion.Add((obj as ScrollTexture).motion);
			} else if (obj is RotateEffect) {
				motion.Add((obj as RotateEffect).velocity);
			} else if (obj as Animator) {
				motion.Add(Vector3.right);
			}

			SetMotion(obj, Vector3.zero, 0);
		}
	}

	void Update() {
		if (fullSpeedAfter <= 0) {
			t1 = elec ? 1 : 0;
			t2 = elec && cogInPlace ? 1 : 0;
		} else {
			t1 = Mathf.MoveTowards(t1, elec ? 1 : 0, Time.deltaTime / fullSpeedAfter);
			t2 = Mathf.MoveTowards(t2, elec && cogInPlace ? 1 : 0, Time.deltaTime / fullSpeedAfter);
		}
		SetMotion(leadingCog, leadingMotion, t1);

		for (int i = 0; i < affected.Length; i++) {
			SetMotion(affected[i], motion[i], t2);
		}
	}

	void OnDropoff() {
		cogInPlace = true;
		leadingCog.transform.rotation = Quaternion.identity;
		t1 = t2 = 0;
	}

	void OnElectrifyStart() {
		elec = true;print("yes");
	}

	void OnElectrifyEnd() {
		elec = false;
	}

	void SetMotion(Object obj, Vector3 motion, float t) {
		if (t == 0) motion = Vector3.zero;
		else motion = Vector3.Lerp(Vector3.zero, motion, t);

		if (obj is ScrollTexture) {
			(obj as ScrollTexture).motion = motion;
		} else if (obj is RotateEffect) {
			(obj as RotateEffect).velocity = motion;
		} else if (obj is Animator) {
			(obj as Animator).SetFloat("Speed", t);
		}
	}
	
}
