using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class Gate : MonoBehaviour {

	public _DropoffStation station;
	public Animator anim;
	public ParticleSystem[] particles;
	public Cog[] cogs;
	[Tooltip("Degrees/second")]
	public float cogTopSpeed = 1;
	[Tooltip("Degrees/second2")]
	public float cogAcc = 1;

	private bool playAnim = false;
	private bool rotCogs = false;
	private float cogSpeed = 0;

	void Start() {
		anim.speed = 0;
	}

	void Update() {
		anim.speed = Mathf.MoveTowards(anim.speed, playAnim ? 1 : 0, Time.deltaTime);
		cogSpeed = Mathf.MoveTowards(cogSpeed, rotCogs ? cogTopSpeed : 0, Time.deltaTime * cogAcc);

		foreach (var cog in cogs) {
			if (!cog.requiresLostCog || (cog.requiresLostCog && station.valid)) {
				cog.transform.Rotate(cog.rotationMultilpier * cogSpeed * Time.deltaTime, Space.Self);
			}
		}
	}

	void OnElectrifyStart() {
		if (station.valid)
			playAnim = true;
		rotCogs = true;

		particles.Play();
	}

	void OnElectrifyEnd() {
		playAnim = false;
		rotCogs = false;

		particles.Stop();
	}

	// Called by CogDropoffStation
	public void OnCogDropoff() {
		// Reset all cogs
		foreach (var cog in cogs) {
			var euler = cog.transform.localEulerAngles;
			euler.x = 0;
			cog.transform.localEulerAngles = euler;
		}
	}

	[System.Serializable]
	public struct Cog {
		public Transform transform;
		public Vector3 rotationMultilpier;
		public bool requiresLostCog;
	}
}
