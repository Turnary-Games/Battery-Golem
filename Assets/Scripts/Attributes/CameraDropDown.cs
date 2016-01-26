using UnityEngine;

public class CameraDropDown : PropertyAttribute {
	
	public bool fullHierarchy;

	public CameraDropDown(bool fullHierarchy = false) {
		this.fullHierarchy = fullHierarchy;
	}

}