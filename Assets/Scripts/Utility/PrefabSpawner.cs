using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour {

	public GameObject prefab;
	public Vector3 spawnOffset;
	public Space offsetRelativeTo = Space.World; 
	[Space]
	public float delay = 2f;
	public bool randomStartDelay = true;
	private float timeLeft = 0f;

	#if UNITY_EDITOR
	void OnValidate() {
		delay = Mathf.Max(delay, 0.001f);
	}

	void OnDrawGizmosSelected() {
		Vector3 spawnpos = transform.position + (offsetRelativeTo == Space.Self ? transform.TransformVector(spawnOffset) : spawnOffset);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, spawnpos);
		Gizmos.DrawSphere(spawnpos, .3f);
	}
#endif

	void Start() {
		if (randomStartDelay)
			timeLeft += Random.Range(0, delay);
	}

	void Update() {
		if (timeLeft > 0) {
			timeLeft = Mathf.Max(timeLeft - Time.deltaTime, 0);

			if (timeLeft <= 0) {
				timeLeft += delay;

				Vector3 pos = transform.position;
				if (offsetRelativeTo == Space.World) pos += spawnOffset;
				else if (offsetRelativeTo == Space.Self) pos += transform.TransformVector(spawnOffset);

				GameObject clone = Instantiate(prefab, pos, prefab.transform.rotation) as GameObject;
				clone.transform.SetParent(transform);
			}
		}
	}
}
