using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class Fan : _CoreItem {

	[Header("Fan fields")]

	[Tooltip("Top speed the lilypad will reach.")]
	public float speed = 1;
	[Tooltip("Similar to acceleration.")]
	public float power = 1;

	public ParticleSystem particles;
	public RotateEffect rotator;
	public Renderer[] nearbyVisuals;

	protected override void Start() {
		base.Start();
		SetParticleEmission(false);
	}

	void Update() {
		if (nearbyVisual) {
			foreach (var ren in nearbyVisuals) {
				if (ren) {
					ren.enabled = nearbyVisual.enabled;
				}
			}
		}
	}

	void OnElectrify() {
		// Error checking
		if (inventory == null)
			return;

		Lilypad lilypad = inventory.movement.platform as Lilypad;
		if (lilypad != null) {
			// Move objects
			Vector3 move = -transform.forward * speed;
			lilypad.Move(move, power);
		}
	}

	void OnElectrifyStart() {
		SetParticleEmission(true);
		rotator.enabled = true;
	}

	void OnElectrifyEnd() {
		SetParticleEmission(false);
		rotator.enabled = false;
	}

	void SetParticleEmission(bool state) {
		if (particles == null) return;

		foreach(var ps in particles.GetComponentsInChildren<ParticleSystem>()) {
			var em = ps.emission;
			em.enabled = state;
			if (state && !ps.isPlaying) ps.Play();
		}
	}

}
