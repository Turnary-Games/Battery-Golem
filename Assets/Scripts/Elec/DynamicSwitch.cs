using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class DynamicSwitch : MonoBehaviour {

	public Component targetObject;
	public Type type;
	[Space]
	public bool inverted;
	public bool recursive;

	[HideInInspector]
	public Filter filter;

	// Particle systems specific
	[HideInInspector]
	public ParticleSwitchType particleSwitch;

	void Start() {
		SetState(inverted);
	}

	void OnElectrifyStart() {
		SetState(!inverted);
	}

	void OnElectrifyEnd() {
		SetState(inverted);
	}

	void SetState(bool state) {
		if (recursive) {
			if (type == Type.particleSystem) SetStateLoop<ParticleSystem>(ps => {
				if (particleSwitch == ParticleSwitchType.emission) {
					var em = ps.emission;
					em.enabled = state;
				} else if (particleSwitch == ParticleSwitchType.playPause) {
					if (ps.isPaused) ps.Play();
					else if (ps.isPlaying) ps.Pause();
				} else if (particleSwitch == ParticleSwitchType.playStop) {
					if (ps.isStopped) ps.Play();
					else if (ps.isPlaying) ps.Stop();
				}
			});
			else if (type == Type.monoBehaviour) SetStateLoop<MonoBehaviour>(script => script.enabled = state);
			else if (type == Type.rigidbody) SetStateLoop<Rigidbody>(body => body.SetEnabled(state));
			else SetStateLoop<Transform>(t => t.gameObject.SetActive(state));

		} else {
			if (type == Type.particleSystem && targetObject is ParticleSystem) {
				var ps = targetObject as ParticleSystem;

				if (particleSwitch == ParticleSwitchType.emission) {
					var em = ps.emission;
					em.enabled = state;
				} else if (particleSwitch == ParticleSwitchType.playPause) {
					if (ps.isPaused) ps.Play();
					else if (ps.isPlaying) ps.Pause();
				} else if (particleSwitch == ParticleSwitchType.playStop) {
					if (ps.isStopped) ps.Play();
					else if (ps.isPlaying) ps.Stop();
				}
			}
			else if (type == Type.monoBehaviour && targetObject is MonoBehaviour) (targetObject as MonoBehaviour).enabled = state;
			else if (type == Type.rigidbody) (targetObject as Rigidbody).SetEnabled(state);
			else if (type == Type.gameObject) targetObject.gameObject.SetActive(state);
		}
	}

	void SetStateLoop<T>(System.Action<T> callback) where T : Component {
		foreach(var o in targetObject.GetComponentsInChildren<T>(true)) {
			if (filter == Filter.mustBeSameType) {
				if (o.GetType() == targetObject.GetType())
					callback(o);
			} else
				callback(o);
		}
	}

	public enum Type {
		gameObject, particleSystem, rigidbody, monoBehaviour
	}

	public enum Filter {
		noFilter, mustBeSameType
	}

	public enum ParticleSwitchType {
		emission, playPause, playStop
	}

}
