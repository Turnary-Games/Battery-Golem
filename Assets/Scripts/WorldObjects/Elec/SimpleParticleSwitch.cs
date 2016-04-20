using UnityEngine;
using System.Collections;

public class SimpleParticleSwitch : MonoBehaviour {

	public Object targetObject;
	public bool inverted;
	public bool recursive;

	void Start() {
		SetParticlesActive(inverted);
	}

	void OnElectrifyStart() {
		SetParticlesActive(!inverted);
	}

	void OnElectrifyEnd() {
		SetParticlesActive(inverted);
	}

	void SetParticlesActive(ParticleSystem ps, bool state) {
		var em = ps.emission;
		em.enabled = state;
	}

	void SetParticlesActive(bool state) {
		GameObject go = null;

		if (targetObject is GameObject) {
			go = targetObject as GameObject;
		} else if (targetObject is Transform) {
			go = (targetObject as Transform).gameObject;
		} else if (targetObject is Component) {
			go = (targetObject as Component).gameObject;
		}

		if (go != null) {
			if (recursive) {
				foreach (var ps in go.GetComponentsInChildren<ParticleSystem>(true))
					SetParticlesActive(ps, state);
			} else {
				foreach (var ps in go.GetComponents<ParticleSystem>())
					SetParticlesActive(ps, state);
			}
		}
	}
}
