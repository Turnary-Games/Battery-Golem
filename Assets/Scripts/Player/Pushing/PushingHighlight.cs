using UnityEngine;
using System.Collections;

public class PushingHighlight : MonoBehaviour {

	public Renderer[] renderers;
	
	public void SetHighlightActive(bool state) {
		foreach (Renderer ren in renderers) {
			if (ren)
				ren.enabled = state;
		}
	}

}
