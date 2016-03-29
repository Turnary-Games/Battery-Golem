using UnityEngine;
using System.Collections;

public class AveragePos : MonoBehaviour {

	public Transform[] list;

	void LateUpdate() {
		if (list.Length > 0) {

			Vector3[] collection = new Vector3[list.Length];
			for (int i = 0; i < list.Length; i++) {
				if (!list[i]) continue;
				collection[i] = list[i].position;
			}

			transform.position = VectorHelper.Average(vectors: collection);
		}

	}
}
