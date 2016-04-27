using UnityEngine;
using System.Collections;

public class GUILeaf : MonoBehaviour {

	public Transform spawnPreviewAt;
	[HideInInspector]
	public GameObject previewPrefab;
	
	public void SpawnPreviewModel() {
		if (previewPrefab == null) return;
		
		GameObject clone = Instantiate(previewPrefab);
		clone.transform.SetParent(spawnPreviewAt);
		clone.transform.localPosition = previewPrefab.transform.localPosition;
		clone.transform.localRotation = previewPrefab.transform.localRotation;
		clone.transform.localScale = previewPrefab.transform.localScale;
	}

}
